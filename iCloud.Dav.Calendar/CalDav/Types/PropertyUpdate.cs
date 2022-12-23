using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.CalDav.Types
{
    [XmlRoot(ElementName = "propertyupdate", Namespace = "DAV:")]
    internal sealed class PropertyUpdate : IXmlSerializable
    {
        public string DisplayName { get; }

        public string CalendarColor { get; }

        public string Order { get; }

        public PropertyUpdate(string displayName, string calendarColor, string order)
        {
            DisplayName = displayName;
            CalendarColor = calendarColor;
            Order = order;
        }

        public XmlSchema GetSchema() => new XmlSchema();

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("set", "DAV:");
            writer.WriteStartElement("prop", "DAV:");

            writer.WriteElementString("displayname", "DAV:", DisplayName);

            if (!string.IsNullOrEmpty(CalendarColor))
            {
                writer.WriteElementString("calendar-color", "http://apple.com/ns/ical/", CalendarColor);
            }

            if (!string.IsNullOrEmpty(Order))
            {
                writer.WriteElementString("calendar-order", "http://apple.com/ns/ical/", Order);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}