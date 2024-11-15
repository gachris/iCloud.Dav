namespace iCloud.Dav.Core.Extensions;

/// <summary>
/// Provides extension methods for performing argument validation checks.
/// </summary>
public static class NullableExtensions
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the specified struct object is null.
    /// </summary>
    /// <typeparam name="T">The type of the struct.</typeparam>
    /// <param name="obj">The struct object to check for null.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>The non-null struct object.</returns>
    public static T ThrowIfNull<T>(this T? obj, string paramName) where T : struct
    {
        return obj == null ? throw new ArgumentNullException(paramName) : (T)obj;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the specified class object is null.
    /// </summary>
    /// <typeparam name="T">The type of the class.</typeparam>
    /// <param name="obj">The class object to check for null.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>The non-null class object.</returns>
    public static T ThrowIfNull<T>(this T obj, string paramName) where T : class
    {
        return obj == null ? throw new ArgumentNullException(paramName) : obj;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the specified string is null or empty.
    /// </summary>
    /// <param name="str">The string to check for null or empty.</param>
    /// <param name="paramName">The name of the parameter.</param>
    /// <returns>The non-null and non-empty string.</returns>
    public static string ThrowIfNullOrEmpty(this string str, string paramName)
    {
        return string.IsNullOrEmpty(str) ? throw new ArgumentException("Parameter was empty", paramName) : str;
    }
}