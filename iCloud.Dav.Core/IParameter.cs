namespace iCloud.Dav.Core;

/// <summary>Represents a parameter for a method.</summary>
public interface IParameter
{
    /// <summary>Gets the name of the parameter.</summary>
    string Name { get; }

    /// <summary>Gets the pattern that this parameter must follow.</summary>
    string Pattern { get; }

    /// <summary>Gets an indication whether this parameter is optional or required.</summary>
    bool IsRequired { get; }

    /// <summary>Gets the default value of this parameter.</summary>
    string DefaultValue { get; }

    /// <summary>Gets the type of the parameter.</summary>
    string ParameterType { get; }
}
