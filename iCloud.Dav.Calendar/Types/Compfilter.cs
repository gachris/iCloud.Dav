using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "comp-filter", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class Compfilter
    {
        [XmlElement(ElementName = "time-range", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public Timerange Timerange { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "comp-filter", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public Compfilter Child { get; set; }
    }
}
