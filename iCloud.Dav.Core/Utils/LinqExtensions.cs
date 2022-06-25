using System;
using System.Collections;
using System.Linq;

namespace iCloud.Dav.Core.Utils
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable enumerable, Action<T> action)
        {
            foreach (var item in enumerable.OfType<T>()) action.Invoke(item);
        }
    }
}
