using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    [XmlRoot(ElementName = "propstat", Namespace = "DAV:")]
    public class Propstat
    {
        [XmlElement(ElementName = "prop", Namespace = "DAV:")]
        public Prop Prop { get; set; }

        [XmlElement(ElementName = "status", Namespace = "DAV:")]
        public string Status { get; set; }
    }
}
