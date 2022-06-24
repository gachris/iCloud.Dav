using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "schedule-outbox-URL", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class ScheduleoutboxURL
    {
        public static ScheduleoutboxURL Default = new ScheduleoutboxURL();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
