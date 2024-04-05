using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.Dav.Core.Extensions;

/// <summary>
/// Provides extension methods for working with LINQ and collections.
/// </summary>
internal static class LinqExtensions
{
    /// <summary>
    /// Performs the specified action on each element in the enumerable that is of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements to process.</typeparam>
    /// <param name="enumerable">The enumerable to process.</param>
    /// <param name="action">The action to perform on each element.</param>
    public static void ForEach<T>(this IEnumerable enumerable, Action<T> action)
    {
        foreach (var item in enumerable.OfType<T>()) action.Invoke(item);
    }

    /// <summary>
    /// Performs the specified action on each element in the enumerable.
    /// </summary>
    /// <typeparam name="T">The type of the elements to process.</typeparam>
    /// <param name="enumerable">The enumerable to process.</param>
    /// <param name="action">The action to perform on each element.</param>
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable) action.Invoke(item);
    }

    /// <summary>
    /// Performs the specified action on each element in the list.
    /// </summary>
    /// <typeparam name="T">The type of the elements to process.</typeparam>
    /// <param name="enumerable">The list to process.</param>
    /// <param name="action">The action to perform on each element.</param>
    public static void ForEach<T>(this IList<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable) action.Invoke(item);
    }

    /// <summary>
    /// Performs the specified action on each element in the array.
    /// </summary>
    /// <param name="array">The array to process.</param>
    /// <param name="action">The action to perform on each element.</param>
    public static void ForEach(this Array array, Action<object> action)
    {
        foreach (var item in array) action.Invoke(item);
    }
}