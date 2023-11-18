using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Auth.WebDav.DataTypes.Cal;

/// <summary>
/// Represents the properties associated with a response.
/// </summary>
internal class Prop : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets the URL of the principal associated with the resource.
    /// </summary>
    public CurrentUserPrincipal CurrentUserPrincipal { get; set; }

    /// <summary>
    /// Gets or sets the display name associated with the property.
    /// </summary>
    public DisplayName DisplayName { get; set; }

    /// <summary>
    /// Gets the URL of the calendar home collection for the principal.
    /// </summary>
    public CalendarHomeSet CalendarHomeSet { get; set; }

    /// <summary>
    /// Gets the set of calendar user addresses associated with the resource.
    /// </summary>
    public CalendarUserAddressSet CalendarUserAddressSet { get; set; }

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
        if (!reader.IsStartElement("prop")) return;

        while (reader.Read())
        {
            if (reader.IsStartElement("displayname") && reader.Depth == 4)
            {
                if (reader.IsEmptyElement) continue;

                DisplayName = new DisplayName();
                DisplayName.ReadXml(reader);
            }
            else if (reader.IsStartElement("calendar-home-set", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
            {
                if (reader.IsEmptyElement) continue;

                CalendarHomeSet = new CalendarHomeSet();
                CalendarHomeSet.ReadXml(reader);
            }
            else if (reader.IsStartElement("calendar-user-address-set", "urn:ietf:params:xml:ns:caldav") && reader.Depth == 4)
            {
                if (reader.IsEmptyElement) continue;

                CalendarUserAddressSet = new CalendarUserAddressSet();
                CalendarUserAddressSet.ReadXml(reader);
            }
            else if (reader.IsStartElement("current-user-principal") && reader.Depth == 4)
            {
                if (reader.IsEmptyElement) continue;

                CurrentUserPrincipal = new CurrentUserPrincipal();
                CurrentUserPrincipal.ReadXml(reader);
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "prop" && reader.Depth == 3)
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
        writer.WriteStartElement("prop", "DAV:");

        DisplayName?.WriteXml(writer);
        CurrentUserPrincipal?.WriteXml(writer);
        CalendarHomeSet?.WriteXml(writer);
        CalendarUserAddressSet?.WriteXml(writer);

        writer.WriteEndElement();
    }

    #endregion
}