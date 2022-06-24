using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "supported-calendar-component-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class SupportedCalendarComponentSet
    {
        public static SupportedCalendarComponentSet Default = new SupportedCalendarComponentSet();

        [XmlElement(ElementName = "comp", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public List<Comp> Comps { get; set; }
    }
}
