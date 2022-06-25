using iCloud.Dav.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace iCloud.Dav.Core.Utils
{
    public static class Utilities
    {
        /// <summary>Returns the version of the core library.</summary>
        public static string GetLibraryVersion()
        {
            return Regex.Match(typeof(Utilities).Assembly.FullName, "Version=([\\d\\.]+)").Groups[1].ToString();
        }

        /// <summary>
        /// A ICloud.Api utility method for throwing an <see cref="T:System.ArgumentNullException" /> if the object is
        /// <c>null</c>.
        /// </summary>
        public static T ThrowIfNull<T>(this T obj, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
            return obj;
        }

        /// <summary>
        /// A ICloud.Api utility method for throwing an <see cref="T:System.ArgumentNullException" /> if the string is
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
            if (coll != null)
                return !coll.Any();
            return true;
        }

        /// <summary>
        /// A ICloud.Api utility method for returning the first matching custom attribute (or <c>null</c>) of the specified member.
        /// </summary>
        public static T GetCustomAttribute<T>(this MemberInfo info) where T : Attribute
        {
            object[] customAttributes = info.GetCustomAttributes(typeof(T), false);
            if (customAttributes.Length != 0)
                return (T)customAttributes[0];
            return default;
        }

        /// <summary>Returns the defined string value of an Enum.</summary>
        internal static string GetStringValue(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            field.ThrowIfNull<FieldInfo>(nameof(value));
            StringValueAttribute customAttribute = field.GetCustomAttribute<StringValueAttribute>();
            if (customAttribute != null)
                return customAttribute.Text;
            throw new ArgumentException(string.Format("Enum value '{0}' does not contain a StringValue attribute", field), nameof(value));
        }

        /// <summary>
        /// Returns the defined string value of an Enum. Use for test purposes or in other ICloud.Api projects.
        /// </summary>
        public static string GetEnumStringValue(Enum value)
        {
            return value.GetStringValue();
        }

        /// <summary>
        /// Tries to convert the specified object to a string. Uses custom type converters if available.
        /// Returns null for a null object.
        /// </summary>
        public static string ConvertToString(object o)
        {
            if (o == null)
                return null;
            if (o.GetType().IsEnum)
            {
                StringValueAttribute customAttribute = o.GetType().GetField(o.ToString()).GetCustomAttribute<StringValueAttribute>();
                if (customAttribute == null)
                    return o.ToString();
                return customAttribute.Text;
            }
            if (o is DateTime time)
                return Utilities.ConvertToRFC3339(time);
            return o.ToString();
        }

        /// <summary>Converts the input date into a RFC3339 string (http://www.ietf.org/rfc/rfc3339.txt).</summary>
        internal static string ConvertToRFC3339(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = date.ToUniversalTime();
            return date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Parses the input string and returns <see cref="T:System.DateTime" /> if the input is a valid
        /// representation of a date. Otherwise it returns <c>null</c>.
        /// </summary>
        public static DateTime? GetDateTimeFromString(string raw)
        {
            if (!DateTime.TryParse(raw, out DateTime result))
                return new DateTime?();
            return new DateTime?(result);
        }

        /// <summary>Returns a string (by RFC3339) form the input <see cref="T:System.DateTime" /> instance.</summary>
        public static string GetStringFromDateTime(DateTime? date)
        {
            if (!date.HasValue)
                return null;
            return Utilities.ConvertToRFC3339(date.Value);
        }
    }
}