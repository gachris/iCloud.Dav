using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "report", Namespace = "DAV:")]
    public sealed class Report
    {
        [XmlElement(ElementName = "sync-collection", Namespace = "DAV:")]
        public string Synccollection { get; set; }

        [XmlElement(ElementName = "addressbook-multiget", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public Addressbookmultiget Addressbookmultiget { get; set; }
    }
}
