using System;

namespace iCloud.Dav.Core.Serialization;

/// <summary>
/// Specifies the type to be used when deserializing an object from XML format.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class XmlDeserializeTypeAttribute : Attribute
{
    /// <summary>
    /// Gets the type to be used when deserializing an object from XML format.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlDeserializeTypeAttribute"/> class with the specified type.
    /// </summary>
    /// <param name="type">The type to be used when deserializing an object from XML format.</param>
    public XmlDeserializeTypeAttribute(Type type) : base()
    {
        Type = type;
    }
}