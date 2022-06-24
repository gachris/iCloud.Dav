using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "principal-URL", Namespace = "DAV:")]
    public class PrincipalURL
    {
        public static PrincipalURL Default = new PrincipalURL();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
