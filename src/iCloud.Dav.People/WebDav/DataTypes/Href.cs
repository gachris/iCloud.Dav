using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.WebDav.DataTypes;

/// <summary>
/// Represents a hyperlink reference (href) in the context of CalDAV operations.
/// </summary>
internal class Href : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether this calendar user address is preferred.
    /// </summary>
    [XmlAttribute(AttributeName = "preferred")]
    public bool Preferred { get; set; }

    /// <summary>
    /// Gets or sets the value of this calendar user address.
    /// </summary>
    public string Value { get; set; }

    #endregion

    #region IXmlSerializable Implementation

    /// <summary>
    /// Gets the XML schema for this object.
    /// </summary>
    /// <returns>An <see cref="XmlSchema"/> object.</returns>
    public XmlSchema GetSchema() => new XmlSchema();

    /// <summary>
    /// Reads the XML representation of the hyperlink reference.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
    public void ReadXml(XmlReader reader)
    {
        if (!reader.IsStartElement("href")) return;

        int.TryParse(reader.GetAttribute("preferred"), out var preferredNum);
        var preferred = Convert.ToBoolean(preferredNum);

        Preferred = preferred;

        reader.Read();
        Value = reader.ReadContentAsString();
    }

    /// <summary>
    /// Writes the XML representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("href", "DAV:", Value);
    }

    #endregion
}