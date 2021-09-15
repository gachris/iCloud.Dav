using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "schedule-inbox-URL", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class ScheduleinboxURL
    {
        public static ScheduleinboxURL Default = new ScheduleinboxURL();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
