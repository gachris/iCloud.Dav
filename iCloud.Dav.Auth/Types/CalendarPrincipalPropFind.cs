using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    [XmlRoot(ElementName = "propfind", Namespace = "DAV:")]
    internal sealed class CalendarPrincipalPropFind : IXmlSerializable
    {
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
            writer.WriteElementString("calendar-user-address-set", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("calendar-home-set", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("current-user-principal", "DAV:", null);
            writer.WriteElementString("displayname", "DAV:", null);
            writer.WriteFullEndElement();
        }
    }
}
