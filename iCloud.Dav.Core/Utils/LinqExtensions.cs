using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.Dav.Core.Utils;

public static class LinqExtensions
{
    public static void ForEach<T>(this IEnumerable enumerable, Action<T> action)
    {
        foreach (var item in enumerable.OfType<T>()) action.Invoke(item);
    }

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable) action.Invoke(item);
    }

    public static void ForEach<T>(this IList<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable) action.Invoke(item);
    }

    public static void ForEach(this Array array, Action<object> action)
    {
        foreach (var item in array) action.Invoke(item);
    }
}
