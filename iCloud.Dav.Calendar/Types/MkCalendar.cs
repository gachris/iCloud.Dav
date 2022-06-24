using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "mkcalendar", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class MkCalendar
    {
        [XmlElement(ElementName = "set", Namespace = "DAV:")]
        public Set Set { get; set; }
    }
}
