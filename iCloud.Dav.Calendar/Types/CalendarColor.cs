using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "calendar-color", Namespace = "http://apple.com/ns/ical/")]
    public class CalendarColor
    {
        public static readonly CalendarColor Default = new CalendarColor();

        [XmlText]
        public string Value { get; set; }
    }
}
