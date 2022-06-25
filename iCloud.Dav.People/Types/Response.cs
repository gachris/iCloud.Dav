using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "response", Namespace = "DAV:")]
    public sealed class Response<TProp>
    {
        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public string Url { get; set; }

        [XmlElement(ElementName = "propstat", Namespace = "DAV:")]
        public Propstat<TProp> Propstat { get; set; }
    }
}
