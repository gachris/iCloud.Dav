using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "href", Namespace = "DAV:")]
    public class Url
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "preferred")]
        public string Preferred { get; set; }
    }
}
