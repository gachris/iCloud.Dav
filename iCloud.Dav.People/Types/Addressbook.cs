using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "addressbook", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public sealed class Addressbook
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
