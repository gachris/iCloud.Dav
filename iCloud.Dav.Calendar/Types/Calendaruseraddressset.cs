using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "calendar-user-address-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class Calendaruseraddressset
    {
        public static Calendaruseraddressset Default = new Calendaruseraddressset();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public List<Url> Href { get; set; }
    }
}
