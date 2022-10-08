using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Auth.CardDav.Types;

[XmlRoot(ElementName = "propfind", Namespace = "DAV:")]
internal sealed class PropFind : IXmlSerializable
{
    public bool CalendarHomeSet { get; set; }

    public bool CalendarUserAddressSet { get; set; }

    public bool AddressBookHomeSet { get; set; }

    public bool CurrentUserPrincipal { get; set; }

    public bool DisplayName { get; set; }

    public XmlSchema GetSchema()
    {
        return new XmlSchema();
    }

    public void ReadXml(XmlReader reader)
    {
        throw new NotSupportedException();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("prop", "DAV:");

        if (AddressBookHomeSet) writer.WriteElementString("addressbook-home-set", "urn:ietf:params:xml:ns:caldav", null);
        if (CurrentUserPrincipal) writer.WriteElementString("current-user-principal", "DAV:", null);
        if (DisplayName) writer.WriteElementString("displayname", "DAV:", null);
        if (CalendarHomeSet) writer.WriteElementString("calendar-home-set", "urn:ietf:params:xml:ns:caldav", null);
        if (CalendarUserAddressSet) writer.WriteElementString("calendar-user-address-set", "urn:ietf:params:xml:ns:caldav", null);

        writer.WriteFullEndElement();
    }
}