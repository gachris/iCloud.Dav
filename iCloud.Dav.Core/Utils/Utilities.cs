using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace iCloud.Dav.Core.Utils
{
    internal static class Utilities
    {
        /// <summary>
        /// Returns the version of the core library.
        /// </summary>
        public static string GetLibraryVersion() => Regex.Match(input: typeof(Utilities).Assembly.FullName ?? string.Empty, "Version=([\\d\\.]+)").Groups[1].ToString();

        public static T GetCustomAttribute<T>(MemberInfo info) where T : Attribute
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
                var field = o.GetType().GetField(text);
                var customAttribute = field is null ? null : GetCustomAttribute<DescriptionAttribute>(field);
                if (customAttribute == null) return o.ToString();
                return customAttribute.Description;
            }
            if (o is DateTime time) return ConvertToRFC3339(time);
            return o.ToString();
        }

        /// <summary>
        /// Converts the input date into a RFC3339 string (http://www.ietf.org/rfc/rfc3339.txt).
        /// </summary>
        public static string ConvertToRFC3339(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = date.ToUniversalTime();
            return date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", DateTimeFormatInfo.InvariantInfo);
        }
    }
}