using System.Text.RegularExpressions;

namespace iCloud.Dav.Core;

/// <summary>Logic for validating a parameter.</summary>
public static class ParameterValidator
{
    /// <summary>Validates a parameter value against the methods regex.</summary>
    public static bool ValidateRegex(IParameter param, string paramValue)
    {
        if (!string.IsNullOrEmpty(param.Pattern))
            return new Regex(param.Pattern).IsMatch(paramValue);
        return true;
    }

    /// <summary>Validates if a parameter is valid.</summary>
    public static bool ValidateParameter(IParameter parameter, string value)
    {
        if (string.IsNullOrEmpty(value))
            return !parameter.IsRequired;
        return ValidateRegex(parameter, value);
    }
}
