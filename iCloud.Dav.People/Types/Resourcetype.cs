using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "resourcetype", Namespace = "DAV:")]
    public sealed class Resourcetype
    {
        [XmlElement(ElementName = "collection", Namespace = "DAV:")]
        public string Collection { get; set; }

        [XmlElement(ElementName = "addressbook", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public Addressbook Addressbook { get; set; }
    }
}
