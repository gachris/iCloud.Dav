using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "filter", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class Filter
    {
        [XmlElement(ElementName = "comp-filter", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public Compfilter Compfilter { get; set; } = new Compfilter() { };
    }
}
