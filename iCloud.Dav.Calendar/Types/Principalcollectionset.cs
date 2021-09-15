using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "principal-collection-set", Namespace = "DAV:")]
    public class Principalcollectionset
    {
        public static Principalcollectionset Default = new Principalcollectionset();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
