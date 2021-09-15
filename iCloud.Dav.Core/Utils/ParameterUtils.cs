using iCloud.Dav.Core.Attributes;
using iCloud.Dav.Core.Enums;
using iCloud.Dav.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace iCloud.Dav.Core.Utils
{
    /// <summary>
    /// Utility class for iterating on <see cref="T:ICloud.Api.Util.RequestParameterAttribute" /> properties in a request object.
    /// </summary>
    public static class ParameterUtils
    {
        /// <summary>
        /// Creates a <see cref="T:System.Net.Http.FormUrlEncodedContent" /> with all the specified parameters in
        /// the input request. It uses reflection to iterate over all properties with
        /// <see cref="T:ICloud.Api.Util.RequestParameterAttribute" /> attribute.
        /// </summary>
        /// <param name="request">
        /// A request object which contains properties with
        /// <see cref="T:ICloud.Api.Util.RequestParameterAttribute" /> attribute. Those properties will be serialized
        /// to the returned <see cref="T:System.Net.Http.FormUrlEncodedContent" />.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Net.Http.FormUrlEncodedContent" /> which contains the all the given object required
        /// values.
        /// </returns>
        public static FormUrlEncodedContent CreateFormUrlEncodedContent(object request)
        {
            IList<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            ParameterUtils.IterateParameters(request, (type, name, value) => list.Add(new KeyValuePair<string, string>(name, value.ToString())));
            return new FormUrlEncodedContent(list);
        }

        /// <summary>
        /// Creates a parameter dictionary by using reflection to iterate over all properties with
        /// <see cref="T:ICloud.Api.Util.RequestParameterAttribute" /> attribute.
        /// </summary>
        /// <param name="request">
        /// A request object which contains properties with
        /// <see cref="T:ICloud.Api.Util.RequestParameterAttribute" /> attribute. Those properties will be set
        /// in the output dictionary.
        /// </param>
        public static IDictionary<string, object> CreateParameterDictionary(object request)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            ParameterUtils.IterateParameters(request, (type, name, value) => dict.Add(name, value));
            return dict;
        }

        /// <summary>
        /// Sets query parameters in the given builder with all all properties with the
        /// <see cref="T:ICloud.Api.Util.RequestParameterAttribute" /> attribute.
        /// </summary>
        /// <param name="builder">The request builder</param>
        /// <param name="request">
        /// A request object which contains properties with
        /// <see cref="T:ICloud.Api.Util.RequestParameterAttribute" /> attribute. Those properties will be set in the
        /// given request builder object
        /// </param>
        public static void InitParameters(RequestBuilder builder, object request)
        {
            ParameterUtils.IterateParameters(request, (type, name, value) => builder.AddParameter(type, name, value.ToString()));
        }

        /// <summary>
        /// Iterates over all <see cref="T:ICloud.Api.Util.RequestParameterAttribute" /> properties in the request
        /// object and invokes the specified action for each of them.
        /// </summary>
        /// <param name="request">A request object</param>
        /// <param name="action">An action to invoke which gets the parameter type, name and its value</param>
        private static void IterateParameters(object request, Action<RequestParameterType, string, object> action)
        {
            foreach (PropertyInfo property in request.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                RequestParameterAttribute parameterAttribute = ((IEnumerable<object>)property.GetCustomAttributes(typeof(RequestParameterAttribute), false)).FirstOrDefault<object>() as RequestParameterAttribute;
                if (parameterAttribute != null)
                {
                    string str = parameterAttribute.Name ?? property.Name.ToLower();
                    Type propertyType = property.PropertyType;
                    object obj = property.GetValue(request, null);
                    if (propertyType.IsValueType || obj != null)
                        action(parameterAttribute.Type, str, obj);
                }
            }
        }
    }
}
