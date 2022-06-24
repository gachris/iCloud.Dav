using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "time-range", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class Timerange
    {
        [XmlAttribute(AttributeName = "start")]
        public string Start { get; set; }

        [XmlAttribute(AttributeName = "end")]
        public string End { get; set; }
    }
}
