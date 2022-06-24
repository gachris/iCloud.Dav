using System.Xml.Serialization;

namespace iCloud.Dav.ICalendar.Types
{
    [XmlRoot(ElementName = "propstat", Namespace = "DAV:")]
    public class Propstat<TProp>
    {
        [XmlElement(ElementName = "prop", Namespace = "DAV:")]
        public TProp Prop { get; set; }

        [XmlElement(ElementName = "status", Namespace = "DAV:")]
        public string Status { get; set; }
    }
}
