using iCloud.Dav.Core.Logger;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iCloud.Dav.Core
{
    public abstract class BaseClientService : IClientService, IDisposable
    {
        /// <summary>The class logger.</summary>
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<BaseClientService>();

        /// <summary>The default maximum allowed length of a URL string for GET requests.</summary>
        public const uint DefaultMaxUrlLength = 2048;
        
        private readonly SerializationContext _serializationContext;

        public ConfigurableHttpClient HttpClient { get; private set; }

        public IConfigurableHttpClientCredentialInitializer HttpClientInitializer { get; private set; }

        public string ApplicationName { get; private set; }

        public ISerializer Serializer { get; private set; }

        public abstract string Name { get; }

        public abstract string BasePath { get; }

        /// <summary>Constructs a new base client with the specified initializer.</summary>
        protected BaseClientService(Initializer initializer)
        {
            Serializer = initializer.Serializer;
            ApplicationName = initializer.ApplicationName;
            if (ApplicationName == null)
                Logger.Warning("Application name is not set. Please set Initializer.ApplicationName property");
            HttpClientInitializer = initializer.HttpClientInitializer;
            HttpClient = CreateHttpClient(initializer);

            _serializationContext = new SerializationContext(this, "MySampleProp");
            _serializationContext.SetService(HttpClient);
        }

        private ConfigurableHttpClient CreateHttpClient(Initializer initializer)
        {
            var httpClientFactory = initializer.HttpClientFactory ?? new HttpClientFactory();
            var args = new CreateHttpClientArgs() { ApplicationName = ApplicationName };
            if (HttpClientInitializer != null)
                args.Initializers.Add(HttpClientInitializer);
            if (initializer.DefaultExponentialBackOffPolicy != ExponentialBackOffPolicy.None)
                args.Initializers.Add(new ExponentialBackOffInitializer(initializer.DefaultExponentialBackOffPolicy, new Func<BackOffHandler>(CreateBackOffHandler)));
            var httpClient = httpClientFactory.CreateHttpClient(args);
            if (initializer.MaxUrlLength > 0U)
                httpClient.MessageHandler.AddExecuteInterceptor(new MaxUrlLengthInterceptor(initializer.MaxUrlLength));
            return httpClient;
        }

        /// <summary>
        /// Creates the back-off handler with <see cref="ExponentialBackOff" />.
        /// Overrides this method to change the default behavior of back-off handler (e.g. you can change the maximum
        /// waited request's time span, or create a back-off handler with you own implementation of
        /// <see cref="IBackOff" />).
        /// </summary>
        protected virtual BackOffHandler CreateBackOffHandler() => new BackOffHandler(new ExponentialBackOff());

        public void SetRequestSerailizedContent(HttpRequestMessage request, object body) => request.SetRequestSerailizedContent(this, body);

        public virtual string SerializeObject(object obj) => !(obj is string) ? Serializer.Serialize(obj) : (string)obj;

        public virtual async Task<string> DeserializeResponse(HttpResponseMessage response) => await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        public virtual async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var responseContentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (typeof(T).BaseType == typeof(VoidResponse) || typeof(T) == typeof(VoidResponse)) return (T)Activator.CreateInstance(typeof(VoidResponse));
            if (Equals(typeof(T), typeof(string))) return (T)(object)responseContentString;

            var obj = default(object);
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                var xmlDeserializeType = (XmlDeserializeTypeAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(XmlDeserializeTypeAttribute));
                if (xmlDeserializeType != null)
                {
                    var result = Serializer.Deserialize(responseContentString, xmlDeserializeType.Type);
                    if (converter != null)
                    {
                        var canConvert = converter.CanConvertFrom(result.GetType());
                        if (canConvert) obj = converter.ConvertFrom(_serializationContext, null, result);
                    }
                }
                else if (converter != null)
                {
                    var canConvert = converter.CanConvertFrom(responseContentString.GetType());
                    if (canConvert) obj = converter.ConvertFrom(responseContentString);
                }
                else
                {
                    obj = Serializer.Deserialize<T>(responseContentString);
                }

                if (obj is null && typeof(T).Equals(typeof(byte[])))
                {
                    obj = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                }
            }
            catch (InvalidCastException ex)
            {
                throw new ICloudApiException(Name, $"Failed to cast response.", ex);
            }
            catch (Exception ex)
            {
                throw new ICloudApiException(Name, $"Failed to parse response from server [{responseContentString}]", ex);
            }

            if (obj is IDirectResponseSchema directResponseSchema && string.IsNullOrEmpty(response.Headers.ETag?.Tag) == false) directResponseSchema.ETag = response.Headers.ETag?.Tag;

            return (T)obj;
        }

        public virtual async Task<string> DeserializeError(HttpResponseMessage response) => await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);

            if (HttpClient == null)
                return;
            HttpClient.Dispose();
        }

        /// <summary>An initializer class for the client service.</summary>
        public class Initializer
        {
            /// <summary>
            /// Gets or sets the factory for creating <see cref="System.Net.Http.HttpClient" /> instance. If this
            /// property is not set the service uses a new <see cref="Core.HttpClientFactory" /> instance.
            /// </summary>
            public IHttpClientFactory HttpClientFactory { get; set; }

            /// <summary>
            /// Gets or sets a HTTP client initializer which is able to customize properties on
            /// <see cref="ConfigurableHttpClient" /> and
            /// <see cref="ConfigurableMessageHandler" />.
            /// </summary>
            public IConfigurableHttpClientCredentialInitializer HttpClientInitializer { get; }

            /// <summary>
            /// Get or sets the exponential back-off policy used by the service. Default value is
            /// <c>UnsuccessfulResponse503</c>, which means that exponential back-off is used on 503 abnormal HTTP
            /// response.
            /// If the value is set to <c>None</c>, no exponential back-off policy is used, and it's up to the user to
            /// configure the <see cref="ConfigurableMessageHandler" /> in an
            /// <see cref="IConfigurableHttpClientInitializer" /> to set a specific back-off
            /// implementation (using <see cref="BackOffHandler" />).
            /// </summary>
            public ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; set; }

            /// <summary>
            /// Gets or sets the serializer. Default value is <see cref="XmlObjectSerializer" />.
            /// </summary>
            public ISerializer Serializer { get; set; }

            /// <summary>
            /// Gets or sets Application name to be used in the User-Agent header. Default value is <c>null</c>.
            /// </summary>
            public string ApplicationName { get; set; }

            /// <summary>
            /// Maximum allowed length of a URL string for GET requests. Default value is <c>2048</c>. If the value is
            /// set to <c>0</c>, requests will never be modified due to URL string length.
            /// </summary>
            public uint MaxUrlLength { get; set; }

            /// <summary>Constructs a new initializer with default values.</summary>
            public Initializer(IConfigurableHttpClientCredentialInitializer httpClientInitializer)
            {
                HttpClientInitializer = httpClientInitializer;
                Serializer = new XmlObjectSerializer();
                DefaultExponentialBackOffPolicy = ExponentialBackOffPolicy.UnsuccessfulResponse503;
                MaxUrlLength = 2048U;
            }
        }
    }

    public class SerializationContext : ITypeDescriptorContext
    {
        private readonly Stack<WeakReference> _mStack = new Stack<WeakReference>();
        private readonly ServiceProvider _mServiceProvider = new ServiceProvider();

        public SerializationContext(object instance, string propertyName)
        {
            Instance = instance;
            PropertyDescriptor = TypeDescriptor.GetProperties(instance)[propertyName];
        }

        public object Instance { get;  }

        public PropertyDescriptor PropertyDescriptor { get;  }

        public IContainer Container { get; }

        public void OnComponentChanged()
        {
        }

        public bool OnComponentChanging()
        {
            return true;
        }

        public virtual void Push(object item)
        {
            if (item != null)
            {
                _mStack.Push(new WeakReference(item));
            }
        }

        public virtual object Pop()
        {
            if (_mStack.Count > 0)
            {
                var r = _mStack.Pop();
                if (r.IsAlive)
                {
                    return r.Target;
                }
            }
            return null;
        }

        public virtual object Peek()
        {
            if (_mStack.Count > 0)
            {
                var r = _mStack.Peek();
                if (r.IsAlive)
                {
                    return r.Target;
                }
            }
            return null;
        }

        public virtual object GetService(Type serviceType) => _mServiceProvider.GetService(serviceType);

        public virtual object GetService(string name) => _mServiceProvider.GetService(name);

        public virtual T GetService<T>() => _mServiceProvider.GetService<T>();

        public virtual T GetService<T>(string name) => _mServiceProvider.GetService<T>(name);

        public virtual void SetService(string name, object obj) => _mServiceProvider.SetService(name, obj);

        public virtual void SetService(object obj) => _mServiceProvider.SetService(obj);

        public virtual void RemoveService(Type type) => _mServiceProvider.RemoveService(type);

        public virtual void RemoveService(string name) => _mServiceProvider.RemoveService(name);
    }

    public class ServiceProvider
    {
        private readonly IDictionary<Type, object> _mTypedServices = new Dictionary<Type, object>();
        private readonly IDictionary<string, object> _mNamedServices = new Dictionary<string, object>();

        public virtual object GetService(Type serviceType)
        {
            object service;
            _mTypedServices.TryGetValue(serviceType, out service);
            return service;
        }

        public virtual object GetService(string name)
        {
            object service;
            _mNamedServices.TryGetValue(name, out service);
            return service;
        }

        public virtual T GetService<T>()
        {
            var service = GetService(typeof(T));
            if (service is T)
            {
                return (T)service;
            }
            return default;
        }

        public virtual T GetService<T>(string name)
        {
            var service = GetService(name);
            if (service is T)
            {
                return (T)service;
            }
            return default;
        }

        public virtual void SetService(string name, object obj)
        {
            if (!string.IsNullOrEmpty(name) && obj != null)
            {
                _mNamedServices[name] = obj;
            }
        }

        public virtual void SetService(object obj)
        {
            if (obj != null)
            {
                var type = obj.GetType();
                _mTypedServices[type] = obj;

                // Get interfaces for the given type
                foreach (var iface in type.GetInterfaces())
                {
                    _mTypedServices[iface] = obj;
                }
            }
        }

        public virtual void RemoveService(Type type)
        {
            if (type != null)
            {
                if (_mTypedServices.ContainsKey(type))
                {
                    _mTypedServices.Remove(type);
                }

                // Get interfaces for the given type
                foreach (var iface in type.GetInterfaces().Where(iface => _mTypedServices.ContainsKey(iface)))
                {
                    _mTypedServices.Remove(iface);
                }
            }
        }

        public virtual void RemoveService(string name)
        {
            if (_mNamedServices.ContainsKey(name))
            {
                _mNamedServices.Remove(name);
            }
        }
    }
}