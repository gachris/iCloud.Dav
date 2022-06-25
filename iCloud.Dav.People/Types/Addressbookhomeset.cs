using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "addressbook-home-set", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public sealed class Addressbookhomeset
    {
        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
