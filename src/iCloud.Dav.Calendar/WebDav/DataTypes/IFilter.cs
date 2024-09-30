using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.WebDav.DataTypes;

/// <summary>
/// Represents a filter used in constructing XML representations of filters in the CalDAV namespace.
/// </summary>
internal interface IFilter : IXmlSerializable
{
    /// <summary>
    /// Gets the collection of child filter elements.
    /// </summary>
    IEnumerable<IFilter> Children { get; }
}