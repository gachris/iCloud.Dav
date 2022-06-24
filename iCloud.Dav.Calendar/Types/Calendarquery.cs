using iCloud.Dav.Calendar.Types;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Request
{
    [XmlRoot(ElementName = "calendar-query", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class Calendarquery
    {
        [XmlElement(ElementName = "prop", Namespace = "DAV:")]
        public Prop Prop { get; set; }

        [XmlElement(ElementName = "filter", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public Filter Filter { get; set; }
    }
}
