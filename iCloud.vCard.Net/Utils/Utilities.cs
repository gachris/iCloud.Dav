using System;

namespace iCloud.vCard.Net.Utils;

internal static class Utilities
{
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
    public static T ThrowIfNull<T>(this T? obj, string paramName) where T : class
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
    public static string ThrowIfNullOrEmpty(this string? str, string paramName)
    {
        if (string.IsNullOrEmpty(str))
            throw new ArgumentException("Parameter was empty", paramName);
        return str;
    }
}