using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.CalDav.Types
{
    [XmlRoot(ElementName = "propfind", Namespace = "DAV:")]
    internal sealed class PropFind : IXmlSerializable
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
            writer.WriteElementString("displayname", "DAV:", null);
            writer.WriteElementString("getctag", "http://calendarserver.org/ns/", null);
            writer.WriteElementString("getetag", "DAV:", null);
            writer.WriteElementString("supported-report-set", "DAV:", null);
            writer.WriteElementString("supported-calendar-component-set", "urn:ietf:params:xml:ns:caldav", null);
            writer.WriteElementString("current-user-privilege-set", "DAV:", null);
            writer.WriteElementString("calendar-color", "http://apple.com/ns/ical/", null);
            writer.WriteString("calendar-data");
            writer.WriteEndElement();
        }
    }
}
