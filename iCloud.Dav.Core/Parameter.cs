namespace iCloud.Dav.Core;

/// <summary>Represents a method's parameter.</summary>
public class Parameter : IParameter
{
    public string Name { get; }

    public string ParameterType { get; }

    public bool IsRequired { get; }

    public string? Pattern { get; set; }

    public string? DefaultValue { get; set; }

    public Parameter(string name, string parameterType, bool isRequired) => (Name, ParameterType, IsRequired) = (name, parameterType, isRequired);
}
