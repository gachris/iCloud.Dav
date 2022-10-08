namespace iCloud.Dav.Core;

/// <summary>Represents a method's parameter.</summary>
public class Parameter : IParameter
{
    public string Name { get; set; }

    public string Pattern { get; set; }

    public bool IsRequired { get; set; }

    public string ParameterType { get; set; }

    public string DefaultValue { get; set; }
}
