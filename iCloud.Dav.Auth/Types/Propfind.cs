using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    [XmlRoot(ElementName = "propfind", Namespace = "DAV:")]
    public class Propfind
    {
        [XmlElement(ElementName = "prop", Namespace = "DAV:")]
        public Prop Prop { get; set; }
    }
}
