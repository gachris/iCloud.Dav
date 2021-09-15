using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    [XmlRoot(ElementName = "response", Namespace = "DAV:")]
    public class Response<TProp>
    {
        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public string Url { get; set; }

        [XmlElement(ElementName = "propstat", Namespace = "DAV:")]
        public Propstat<TProp> Propstat { get; set; }
    }
}
