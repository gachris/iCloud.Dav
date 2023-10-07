using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.WebDav.DataTypes;

/// <summary>
/// Represents a component filter used in CalDAV operations.
/// </summary>
internal class CompFilter : IXmlSerializable, IFilter
{
    #region Properties

    /// <summary>
    /// Gets or sets the collection of child filter elements.
    /// </summary>
    public IEnumerable<IFilter> Children { get; set; }

    /// <summary>
    /// Gets or sets the name attribute of the "comp-filter" XML element.
    /// </summary>
    public string Name { get; set; }

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
    /// Writes the XML representation of the property update.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("comp-filter", "urn:ietf:params:xml:ns:caldav");
        writer.WriteAttributeString("name", Name);

        Children?.ToList().ForEach(child => child.WriteXml(writer));

        writer.WriteEndElement();
    }

    #endregion
}