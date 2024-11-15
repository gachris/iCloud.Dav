using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.WebDav.DataTypes;

/// <summary>
/// Represents an address book multiget request in the CardDAV namespace.
/// </summary>
[XmlRoot(ElementName = "addressbook-multiget", Namespace = "urn:ietf:params:xml:ns:carddav")]
internal class AddressbookMultiget : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets the hrefs.
    /// </summary>
    public Href[] Hrefs { get; set; }

    /// <summary>
    /// Gets or sets the prop of the addressbook-multiget request.
    /// </summary>
    public Prop Prop { get; set; }

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
    /// Writes the XML representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        Prop?.WriteXml(writer);
        Hrefs?.ToList().ForEach(h => h.WriteXml(writer));
    }

    #endregion
}