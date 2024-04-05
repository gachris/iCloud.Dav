using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.WebDav.DataTypes;

/// <summary>
/// Represents a filter used in CardDAV queries in the CardDAV namespace.
/// </summary>
internal class Filters : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets the test attribute of the filter.
    /// </summary>
    public string Test { get; set; }

    /// <summary>
    /// Gets or sets an array of <see cref="PropFilter"/> objects representing the property filters in the query.
    /// </summary>
    public PropFilter[] PropFilters { get; set; }

    #endregion

    #region IXmlSerializable Implementation

    /// <summary>
    /// Gets the XML schema for this object.
    /// </summary>
    /// <returns>An <see cref="XmlSchema"/> object.</returns>
    public XmlSchema GetSchema() => new XmlSchema();

    /// <summary>
    /// This method is not supported and will throw a <see cref="NotSupportedException"/>.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
    /// <exception cref="NotSupportedException">This method is not supported.</exception>
    public void ReadXml(XmlReader reader) => throw new NotSupportedException();

    /// <summary>
    /// Writes the XML representation of the filter.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("filter", "urn:ietf:params:xml:ns:carddav");

        if (!string.IsNullOrEmpty(Test))
        {
            writer.WriteAttributeString("test", Test);
        }

        if (PropFilters != null)
        {
            foreach (var propFilter in PropFilters)
            {
                propFilter.WriteXml(writer);
            }
        }

        writer.WriteEndElement();
    }

    #endregion
}