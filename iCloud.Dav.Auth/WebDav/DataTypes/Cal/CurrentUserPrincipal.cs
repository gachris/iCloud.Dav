using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Auth.WebDav.DataTypes.Cal;

/// <summary>
/// Represents a current-user-principal property with the "DAV:" namespace.
/// </summary>
internal class CurrentUserPrincipal : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets the href associated with the current user principal property.
    /// </summary>
    /// <remarks>
    /// This element is in the "DAV:" namespace.
    /// </remarks>
    public Href Href { get; set; }

    #endregion

    #region IXmlSerializable Implementation

    /// <summary>
    /// Gets the XML schema for this object.
    /// </summary>
    /// <returns>An <see cref="XmlSchema"/> object.</returns>
    public XmlSchema GetSchema() => new XmlSchema();

    /// <summary>
    /// Reads the XML representation of the multi-status response.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
    public void ReadXml(XmlReader reader)
    {
        if (!reader.IsStartElement("current-user-principal")) return;

        while (reader.Read())
        {
            if (reader.IsStartElement("href", "DAV:"))
            {
                Href = new Href();
                Href.ReadXml(reader);
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "current-user-principal")
            {
                break;
            }
        }
    }

    /// <summary>
    /// Writes the XML representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("current-user-principal", "DAV:");
        Href?.WriteXml(writer);
        writer.WriteEndElement();
    }

    #endregion
}