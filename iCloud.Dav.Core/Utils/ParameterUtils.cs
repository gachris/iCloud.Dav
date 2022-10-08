using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace iCloud.Dav.Core.Utils;

/// <summary>
/// Utility class for iterating on <see cref="RequestParameterAttribute" /> properties in a request object.
/// </summary>
internal static class ParameterUtils
{
    /// <summary>
    /// Creates a <see cref="FormUrlEncodedContent" /> with all the specified parameters in
    /// the input request. It uses reflection to iterate over all properties with
    /// <see cref="RequestParameterAttribute" /> attribute.
    /// </summary>
    /// <param name="request">
    /// A request object which contains properties with
    /// <see cref="RequestParameterAttribute" /> attribute. Those properties will be serialized
    /// to the returned <see cref="FormUrlEncodedContent" />.
    /// </param>
    /// <returns>
    /// A <see cref="FormUrlEncodedContent" /> which contains the all the given object required
    /// values.
    /// </returns>
    public static FormUrlEncodedContent CreateFormUrlEncodedContent(object request)
    {
        var list = new List<KeyValuePair<string, string>>();
        ParameterUtils.IterateParameters(request, (type, name, value) => list.Add(new KeyValuePair<string, string>(name, value.ToString())));
        return new FormUrlEncodedContent(list);
    }

    /// <summary>
    /// Creates a parameter dictionary by using reflection to iterate over all properties with
    /// <see cref="RequestParameterAttribute" /> attribute.
    /// </summary>
    /// <param name="request">
    /// A request object which contains properties with
    /// <see cref="RequestParameterAttribute" /> attribute. Those properties will be set
    /// in the output dictionary.
    /// </param>
    public static IDictionary<string, object> CreateParameterDictionary(object request)
    {
        var dict = new Dictionary<string, object>();
        ParameterUtils.IterateParameters(request, (type, name, value) => dict.Add(name, value));
        return dict;
    }

    /// <summary>
    /// Sets query parameters in the given builder with all all properties with the
    /// <see cref="RequestParameterAttribute" /> attribute.
    /// </summary>
    /// <param name="builder">The request builder</param>
    /// <param name="request">
    /// A request object which contains properties with
    /// <see cref="RequestParameterAttribute" /> attribute. Those properties will be set in the
    /// given request builder object
    /// </param>
    public static void InitParameters(RequestBuilder builder, object request)
    {
        ParameterUtils.IterateParameters(request, (type, name, value) => builder.AddParameter(type, name, value.ToString()));
    }

    /// <summary>
    /// Iterates over all <see cref="RequestParameterAttribute" /> properties in the request
    /// object and invokes the specified action for each of them.
    /// </summary>
    /// <param name="request">A request object</param>
    /// <param name="action">An action to invoke which gets the parameter type, name and its value</param>
    private static void IterateParameters(object request, Action<RequestParameterType, string, object> action)
    {
        foreach (var property in request.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var parameterAttribute = property.GetCustomAttributes(typeof(RequestParameterAttribute), false).FirstOrDefault() as RequestParameterAttribute;
            if (parameterAttribute != null)
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
