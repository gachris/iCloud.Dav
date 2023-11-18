using System;
using System.Collections.Generic;

namespace iCloud.Dav.People.Extensions;

internal static class ParameterExtensions
{
    public static bool TryParse<TEnum>(this IEnumerable<string> collection, out TEnum enumResult) where TEnum : struct
    {
        var success = false;

        int result = 0;

        foreach (var value in collection)
        {
            if (!value.TryParse<TEnum>(out var type)) continue;
            result |= Convert.ToInt32(type);
        }

        object defaultValue = 0;

        enumResult = (TEnum)defaultValue;

        var obj = (TEnum)Enum.ToObject(typeof(TEnum), result);

        if (obj is TEnum enums)
        {
            success = true;
            enumResult = enums;
        }

        return success;
    }

    public static bool TryParse<TEnum>(this string value, out TEnum enumResult) where TEnum : struct
    {
        var success = Enum.TryParse<TEnum>(value, true, out var result);

        object defaultValue = 0;

        enumResult = (TEnum)defaultValue;

        if (result is TEnum enums)
        {
            enumResult = enums;
        }

        return success;
    }
}