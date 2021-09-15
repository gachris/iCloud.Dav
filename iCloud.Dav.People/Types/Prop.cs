using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "prop", Namespace = "DAV:")]
    public class Prop
    {
        [XmlElement(ElementName = "current-user-principal", Namespace = "DAV:")]
        public Currentuserprincipal Currentuserprincipal { get; set; }

        [XmlElement(ElementName = "supported-report-set", Namespace = "DAV:")]
        public Supportedreportset Supportedreportset { get; set; }

        [XmlElement(ElementName = "addressbook-home-set", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public Addressbookhomeset Addressbookhomeset { get; set; }

        [XmlElement(ElementName = "displayname", Namespace = "DAV:")]
        public Displayname Displayname { get; set; }

        [XmlElement(ElementName = "resourcetype", Namespace = "DAV:")]
        public Resourcetype Resourcetype { get; set; }

        [XmlElement(ElementName = "getetag", Namespace = "DAV:")]
        public Getetag Getetag { get; set; }

        [XmlElement(ElementName = "address-data", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public Addressdata Addressdata { get; set; }
    }
}
