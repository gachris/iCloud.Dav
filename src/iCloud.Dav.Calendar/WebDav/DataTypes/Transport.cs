using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.WebDav.DataTypes;

internal class Transport : IXmlSerializable
{
    #region Properties

    public string Type { get; set; }

    public TokenUrl TokenUrl { get; set; }

    public SubscriptionUrl SubscriptionUrl { get; set; }

    public ApsBundleId ApsBundleId { get; set; }

    public CourierServer CourierServer { get; set; }

    public Env Env { get; set; }

    public RefreshInterval RefreshInterval { get; set; }

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
    public void ReadXml(XmlReader reader)
    {
        if (!reader.IsStartElement("transport")) return;

        Type = reader.GetAttribute("type");

        while (reader.Read())
        {
            if (reader.IsStartElement("token-url"))
            {
                if (reader.IsEmptyElement) continue;

                TokenUrl = new TokenUrl();
                TokenUrl.ReadXml(reader);
            }
            else if (reader.IsStartElement("subscription-url"))
            {
                if (reader.IsEmptyElement) continue;

                SubscriptionUrl = new SubscriptionUrl();
                SubscriptionUrl.ReadXml(reader);
            }
            else if (reader.IsStartElement("apsbundleid"))
            {
                if (reader.IsEmptyElement) continue;

                ApsBundleId = new ApsBundleId();
                ApsBundleId.ReadXml(reader);
            }
            else if (reader.IsStartElement("courierserver"))
            {
                if (reader.IsEmptyElement) continue;

                CourierServer = new CourierServer();
                CourierServer.ReadXml(reader);
            }
            else if (reader.IsStartElement("env"))
            {
                if (reader.IsEmptyElement) continue;

                Env = new Env();
                Env.ReadXml(reader);
            }
            else if (reader.IsStartElement("refresh-interval"))
            {
                if (reader.IsEmptyElement) continue;

                RefreshInterval = new RefreshInterval();
                RefreshInterval.ReadXml(reader);
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "transport")
            {
                break;
            }
        }
    }

    /// <summary>
    /// Writes the XML representation of the property update.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("transport", "http://calendarserver.org/ns/", null);
    }

    #endregion
}