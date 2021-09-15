using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    [XmlRoot(ElementName = "propfind", Namespace = "DAV:")]
    public class Propfind<TProp>
    {
        [XmlElement(ElementName = "prop", Namespace = "DAV:")]
        public TProp Prop { get; set; }
    }
}
