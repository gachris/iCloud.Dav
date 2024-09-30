using System.Net;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using iCloud.Dav.Auth.Extensions;

namespace iCloud.Dav.Auth.WebDav.DataTypes.Cal;

/// <summary>
/// Represents the property status in the <see cref="Response"/>.
/// </summary>
internal class PropStat : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets the property associated with the status in the response.
    /// </summary>
    public Prop Prop { get; set; }

    /// <summary>
    /// Gets or sets the status of the response.
    /// </summary>
    /// <remarks>
    /// This element is in the DAV: namespace.
    /// </remarks>
    public string Status { get; set; }

    /// <summary>
    /// Gets the <see cref="HttpStatusCode"/> value parsed from the response status.
    /// </summary>
    /// <remarks>
    /// This property converts the status string to an equivalent <see cref="HttpStatusCode"/> value.
    /// If the conversion fails, it returns null.
    /// </remarks>
    public HttpStatusCode? StatusCode => Status.ToHttpStatusCode();

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
        if (!reader.IsStartElement("propstat")) return;

        while (reader.Read())
        {
            if (reader.IsStartElement("prop"))
            {
                Prop = new Prop();
                Prop.ReadXml(reader);
            }
            else if (reader.IsStartElement("status", "DAV:"))
            {
                reader.Read();
                Status = reader.ReadContentAsString();
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "propstat")
            {
                break;
            }
        }
    }

    /// <summary>
    /// This method is not supported and will throw a <see cref="NotSupportedException"/>.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    /// <exception cref="NotSupportedException">This method is not supported.</exception>
    public void WriteXml(XmlWriter writer) => throw new NotSupportedException();

    #endregion
}