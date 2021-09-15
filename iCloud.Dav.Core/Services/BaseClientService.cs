using iCloud.Dav.Core.Args;
using iCloud.Dav.Core.Attributes;
using iCloud.Dav.Core.Enums;
using iCloud.Dav.Core.Logger;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Serializer;
using iCloud.Dav.Core.Utils;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace iCloud.Dav.Core.Services
{
    public abstract class BaseClientService : IClientService, IDisposable
    {
        /// <summary>The class logger.</summary>
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<BaseClientService>();
        /// <summary>The default maximum allowed length of a URL string for GET requests.</summary>
        public const uint DefaultMaxUrlLength = 2048;

        /// <summary>Constructs a new base client with the specified initializer.</summary>
        protected BaseClientService(BaseClientService.Initializer initializer)
        {
            this.Serializer = initializer.Serializer;
            this.ApplicationName = initializer.ApplicationName;
            if (this.ApplicationName == null)
                BaseClientService.Logger.Warning("Application name is not set. Please set Initializer.ApplicationName property");
            this.HttpClientInitializer = initializer.HttpClientInitializer;
            this.HttpClient = this.CreateHttpClient(initializer);
        }

        private ConfigurableHttpClient CreateHttpClient(BaseClientService.Initializer initializer)
        {
            IHttpClientFactory httpClientFactory = initializer.HttpClientFactory ?? new HttpClientFactory();
            CreateHttpClientArgs args = new CreateHttpClientArgs()
            {
                ApplicationName = this.ApplicationName
            };
            if (this.HttpClientInitializer != null)
                args.Initializers.Add(this.HttpClientInitializer);
            if (initializer.DefaultExponentialBackOffPolicy != ExponentialBackOffPolicy.None)
                args.Initializers.Add(new ExponentialBackOffInitializer(initializer.DefaultExponentialBackOffPolicy, new Func<BackOffHandler>(this.CreateBackOffHandler)));
            ConfigurableHttpClient httpClient = httpClientFactory.CreateHttpClient(args);
            if (initializer.MaxUrlLength > 0U)
                httpClient.MessageHandler.AddExecuteInterceptor(new MaxUrlLengthInterceptor(initializer.MaxUrlLength));
            return httpClient;
        }

        /// <summary>
        /// Creates the back-off handler with <see cref="T:ICloud.Api.Util.ExponentialBackOff" />.
        /// Overrides this method to change the default behavior of back-off handler (e.g. you can change the maximum
        /// waited request's time span, or create a back-off handler with you own implementation of
        /// <see cref="T:ICloud.Api.Util.IBackOff" />).
        /// </summary>
        protected virtual BackOffHandler CreateBackOffHandler()
        {
            return new BackOffHandler(new ExponentialBackOff());
        }

        public ConfigurableHttpClient HttpClient { get; private set; }

        public IConfigurableHttpClientCredentialInitializer HttpClientInitializer { get; private set; }

        public string ApplicationName { get; private set; }

        public void SetRequestSerailizedContent(HttpRequestMessage request, object body)
        {
            request.SetRequestSerailizedContent(this, body);
        }

        public ISerializer Serializer { get; private set; }

        public virtual string SerializeObject(object obj)
        {
            return obj is string ? (string)obj : Serializer.Serialize(obj);
        }

        public virtual async Task<string> DeserializeResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            object input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (typeof(T).BaseType == (typeof(SuccessfulResponseObject)))
                return (T)Activator.CreateInstance(typeof(T));
            if (Equals(typeof(T), typeof(string)))
                return (T)input;
            T obj2 = default;
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                XmlDeserializeTypeAttribute xmlDeserializeType = (XmlDeserializeTypeAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(XmlDeserializeTypeAttribute));
                if (xmlDeserializeType != null)
                {
                    object result = this.Serializer.Deserialize((string)input, xmlDeserializeType.Type);
                    if (converter != null)
                    {
                        bool canConvertFrom = converter.CanConvertFrom(result.GetType());
                        if (canConvertFrom)
                            obj2 = (T)converter.ConvertFrom(result);
                    }
                }
                else if (converter != null)
                {
                    bool canConvertFrom = converter.CanConvertFrom(input.GetType());
                    if (canConvertFrom)
                        obj2 = (T)converter.ConvertFrom(input);
                }
                else
                {
                    obj2 = this.Serializer.Deserialize<T>((string)input);
                }
            }
            catch (InvalidCastException ex)
            {
                throw new ICloudApiException(this.Name, $"Failed to cast response.", ex);
            }
            catch (Exception ex)
            {
                throw new ICloudApiException(this.Name, $"Failed to parse response from server [{input}]", ex);
            }
            string str = response.Headers.ETag?.Tag;
            if (obj2 is IDirectResponseSchema && str != null)
                (obj2 as IDirectResponseSchema).ETag = str;
            return obj2;
        }

        public virtual async Task<string> DeserializeError(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public abstract string Name { get; }

        public abstract string BaseUri { get; }

        public abstract string BasePath { get; }

        public virtual void Dispose()
        {
            if (this.HttpClient == null)
                return;
            this.HttpClient.Dispose();
        }

        /// <summary>An initializer class for the client service.</summary>
        public class Initializer
        {
            /// <summary>
            /// Gets or sets the factory for creating <see cref="T:System.Net.Http.HttpClient" /> instance. If this
            /// property is not set the service uses a new <see cref="T:ICloud.Api.Http.HttpClientFactory" /> instance.
            /// </summary>
            public IHttpClientFactory HttpClientFactory { get; set; }

            /// <summary>
            /// Gets or sets a HTTP client initializer which is able to customize properties on
            /// <see cref="T:ICloud.Api.Http.ConfigurableHttpClient" /> and
            /// <see cref="T:ICloud.Api.Http.ConfigurableMessageHandler" />.
            /// </summary>
            public IConfigurableHttpClientCredentialInitializer HttpClientInitializer { get; set; }

            /// <summary>
            /// Get or sets the exponential back-off policy used by the service. Default value is
            /// <c>UnsuccessfulResponse503</c>, which means that exponential back-off is used on 503 abnormal HTTP
            /// response.
            /// If the value is set to <c>None</c>, no exponential back-off policy is used, and it's up to the user to
            /// configure the <see cref="T:ICloud.Api.Http.ConfigurableMessageHandler" /> in an
            /// <see cref="T:ICloud.Api.Http.IConfigurableHttpClientInitializer" /> to set a specific back-off
            /// implementation (using <see cref="T:ICloud.Api.Http.BackOffHandler" />).
            /// </summary>
            public ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; set; }

            /// <summary>
            /// Gets or sets the serializer. Default value is <see cref="T:ICloud.Api.Json.NewtonsoftJsonSerializer" />.
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
            public Initializer()
            {
                this.Serializer = new XmlSerializer();
                this.DefaultExponentialBackOffPolicy = ExponentialBackOffPolicy.UnsuccessfulResponse503;
                this.MaxUrlLength = 2048U;
            }
        }
    }
}