using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "comp", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class Comp
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
