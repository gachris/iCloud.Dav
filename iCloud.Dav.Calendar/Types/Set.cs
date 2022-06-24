using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "set", Namespace = "DAV:")]
    public class Set
    {
        [XmlElement(ElementName = "prop", Namespace = "DAV:")]
        public Prop Prop { get; set; }
    }
}
