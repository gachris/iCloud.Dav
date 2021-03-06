using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.CalDav.Types
{
    [XmlRoot(ElementName = "propertyupdate", Namespace = "DAV:")]
    internal sealed class PropertyUpdate : IXmlSerializable
    {
        public string DisplayName { get; set; }

        public string CalendarColor { get; set; }

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
            writer.WriteStartElement("set", "DAV:");
            writer.WriteStartElement("prop", "DAV:");

            writer.WriteStartElement("displayname", "DAV:");
            writer.WriteString(DisplayName);
            writer.WriteEndElement();

            writer.WriteStartElement("calendar-color", "http://apple.com/ns/ical/");
            writer.WriteString(CalendarColor);
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
