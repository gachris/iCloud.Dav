using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "calendar-home-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class Calendarhomeset
    {
        public static Calendarhomeset Default = new Calendarhomeset();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
