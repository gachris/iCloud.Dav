namespace iCloud.Dav.Core;

/// <inheritdoc/>
public class Parameter : IParameter
{
    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public string ParameterType { get; }

    /// <inheritdoc/>
    public bool IsRequired { get; }

    /// <inheritdoc/>
    public string Pattern { get; set; }

    /// <inheritdoc/>
    public string DefaultValue { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Parameter"/> class.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="parameterType">The type of the parameter.</param>
    /// <param name="isRequired">Indicates whether the parameter is required or not.</param>
    public Parameter(string name, string parameterType, bool isRequired)
    {
        Name = name;
        ParameterType = parameterType;
        IsRequired = isRequired;
    }
}