using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "notification-URL", Namespace = "http://calendarserver.org/ns/")]
    public class NotificationURL
    {
        public static NotificationURL Default = new NotificationURL();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
