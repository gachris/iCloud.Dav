using iCloud.Dav.People.Serialization;
using System;
using System.Collections.Generic;

namespace iCloud.Dav.People.Utils;

internal static class SubpropertyExtensions
{
    public static bool TryParse<T>(this IEnumerable<Subproperty> subproperties, out T enumResult) where T : Enum
    {
        var success = false;

        int result = 0;

        foreach (var subproperty in subproperties)
        {
            if (!subproperty.TryParse<T>(out var phoneType)) continue;
            result |= Convert.ToInt32(phoneType);
        }

        object defaultValue = 0;

        enumResult = (T)defaultValue;

        var val = (T)Enum.ToObject(typeof(T), result);

        if (val is T enums)
        {
            success = true;
            enumResult = enums;
        }

        return success;
    }

    public static bool TryParse<T>(this Subproperty subproperty, out T enumResult) where T : Enum
    {
        var types = string.IsNullOrEmpty(subproperty.Value) ? subproperty.Name : subproperty.Value;
        var success = Enum.TryParse(typeof(T), types, true, out var result);

        object defaultValue = 0;

        enumResult = (T)defaultValue;

        if (result is T enums)
        {
            enumResult = enums;
        }

        return success;
    }
}
