using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "propstat", Namespace = "DAV:")]
    public sealed class Propstat<TProp>
    {
        [XmlElement(ElementName = "prop", Namespace = "DAV:")]
        public TProp Prop { get; set; }

        [XmlElement(ElementName = "status", Namespace = "DAV:")]
        public string Status { get; set; }
    }
}
