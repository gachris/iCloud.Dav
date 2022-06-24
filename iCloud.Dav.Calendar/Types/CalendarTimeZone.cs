using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "calendar-timezone", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class CalendarTimeZone
    {
        [XmlText]
        public string Value { get; set; }
    }
}
