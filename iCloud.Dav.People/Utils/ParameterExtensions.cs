using System;
using System.Collections.Generic;

namespace iCloud.Dav.People.Utils;

internal static class ParameterExtensions
{
    public static bool TryParse<T>(this IEnumerable<string> collection, out T enumResult) where T : Enum
    {
        var success = false;

        int result = 0;

        foreach (var value in collection)
        {
            if (!value.TryParse<T>(out var type)) continue;
            result |= Convert.ToInt32(type);
        }

        object defaultValue = 0;

        enumResult = (T)defaultValue;

        var obj = (T)Enum.ToObject(typeof(T), result);

        if (obj is T enums)
        {
            success = true;
            enumResult = enums;
        }

        return success;
    }

    public static bool TryParse<T>(this string value, out T enumResult) where T : Enum
    {
        var success = Enum.TryParse(typeof(T), value, true, out var result);

        object defaultValue = 0;

        enumResult = (T)defaultValue;

        if (result is T enums)
        {
            enumResult = enums;
        }

        return success;
    }
}
