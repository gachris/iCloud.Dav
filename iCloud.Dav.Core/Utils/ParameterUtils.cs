using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iCloud.Dav.Core.Utils
{
    /// <summary>
    /// Utility class for iterating on properties in a request object with the <see cref="RequestParameterAttribute"/> attribute.
    /// </summary>
    internal static class ParameterUtils
    {
        /// <summary>
        /// Creates a dictionary of parameters from the properties in the request object that have the <see cref="RequestParameterAttribute"/> attribute.
        /// </summary>
        /// <param name="request">A request object that contains properties with the <see cref="RequestParameterAttribute"/> attribute. These properties will be added to the output dictionary.</param>
        /// <returns>A dictionary of parameter values.</returns>
        public static IDictionary<string, object> CreateParameterDictionary(object request)
        {
            var dict = new Dictionary<string, object>();
            IterateParameters(request, (type, name, value) => dict.Add(name, value));
            return dict;
        }

        /// <summary>
        /// Iterates over all properties in the request object with the <see cref="RequestParameterAttribute"/> attribute and invokes the specified action for each.
        /// </summary>
        /// <param name="request">A request object.</param>
        /// <param name="action">An action to invoke for each property, which receives the parameter type, name, and value.</param>
        private static void IterateParameters(object request, Action<RequestParameterType, string, object> action)
        {
            foreach (var property in request.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.GetCustomAttributes(typeof(RequestParameterAttribute), false).FirstOrDefault() is RequestParameterAttribute parameterAttribute)
                {
                    var str = parameterAttribute.Name ?? property.Name.ToLower();
                    var propertyType = property.PropertyType;
                    var obj = property.GetValue(request, null);
                    if (propertyType.IsValueType || obj != null)
                        action(parameterAttribute.Type, str, obj);
                }
            }
        }
    }
}