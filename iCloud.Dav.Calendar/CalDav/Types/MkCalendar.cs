using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.CalDav.Types
{
    [XmlRoot(ElementName = "mkcalendar", Namespace = "urn:ietf:params:xml:ns:caldav")]
    internal sealed class MkCalendar : IXmlSerializable
    {
        public string DisplayName { get; }

        public string CalendarColor { get; set; }

        public string Order { get; set; }

        public List<string> SupportedCalendarComponents { get; }

        public string CalendarTimeZone { get; set; }

        public MkCalendar(string displayName)
        {
            DisplayName = displayName;
            SupportedCalendarComponents= new List<string>();
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

            if (SupportedCalendarComponents.Any())
            {
                writer.WriteStartElement("supported-calendar-component-set", "urn:ietf:params:xml:ns:caldav");
                SupportedCalendarComponents.ForEach<string>(comp =>
                {
                    writer.WriteStartElement("comp", "urn:ietf:params:xml:ns:caldav");
                    writer.WriteAttributeString("name", comp);
                    writer.WriteEndElement();
                });
                writer.WriteEndElement();
            }

            if (!string.IsNullOrEmpty(CalendarTimeZone))
            {
                writer.WriteElementString("calendar-timezone", "urn:ietf:params:xml:ns:caldav", CalendarTimeZone);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}