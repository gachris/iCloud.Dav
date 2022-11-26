using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace iCloud.Dav.Core.Utils
{
    public static class Utilities
    {
        /// <summary>Returns the version of the core library.</summary>
        public static string GetLibraryVersion() => Regex.Match(input: typeof(Utilities).Assembly.FullName ?? string.Empty, "Version=([\\d\\.]+)").Groups[1].ToString();

        /// <summary>
        /// A ICloud.Api utility method for throwing an <see cref="ArgumentNullException" /> if the object is
        /// <c>null</c>.
        /// </summary>
        public static T ThrowIfNull<T>(this T? obj, string paramName) where T : struct
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
            return (T)obj;
        }

        /// <summary>
        /// A ICloud.Api utility method for throwing an <see cref="ArgumentNullException" /> if the object is
        /// <c>null</c>.
        /// </summary>
        public static T ThrowIfNull<T>(this T obj, string paramName) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
            return obj;
        }

        /// <summary>
        /// A ICloud.Api utility method for throwing an <see cref="ArgumentNullException" /> if the string is
        /// <c>null</c> or empty.
        /// </summary>
        /// <returns>The original string.</returns>
        public static string ThrowIfNullOrEmpty(this string str, string paramName)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("Parameter was empty", paramName);
            return str;
        }

        /// <summary>Returns <c>true</c> in case the enumerable is <c>null</c> or empty.</summary>
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> coll)
        {
            if (coll != null) return !coll.Any();
            return true;
        }

        /// <summary>
        /// A ICloud.Api utility method for returning the first matching custom attribute (or <c>null</c>) of the specified member.
        /// </summary>
        public static T GetCustomAttribute<T>(this MemberInfo info) where T : Attribute
        {
            var customAttributes = info.GetCustomAttributes(typeof(T), false);
            if (customAttributes.Length != 0) return (T)customAttributes[0];
            return default;
        }

        /// <summary>
        /// Tries to convert the specified object to a string. Uses custom type converters if available.
        /// Returns null for a null object.
        /// </summary>
        public static string ConvertToString(object o)
        {
            if (o == null) return null;
            if (o.GetType().IsEnum && o.ToString() is string text)
            {
                var customAttribute = o.GetType().GetField(text)?.GetCustomAttribute<DescriptionAttribute>();
                if (customAttribute == null) return o.ToString();
                return customAttribute.Description;
            }
            if (o is DateTime time) return ConvertToRFC3339(time);
            return o.ToString();
        }

        /// <summary>Converts the input date into a RFC3339 string (http://www.ietf.org/rfc/rfc3339.txt).</summary>
        internal static string ConvertToRFC3339(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = date.ToUniversalTime();
            return date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", DateTimeFormatInfo.InvariantInfo);
        }
    }
}