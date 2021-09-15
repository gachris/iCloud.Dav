using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    /// <summary>
    /// Addressbook home set.
    /// </summary>
    [XmlRoot(ElementName = "addressbook-home-set", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public class AddressBookHomeSet
    {
        /// <summary>
        /// 
        /// </summary>
        public static AddressBookHomeSet Default = new AddressBookHomeSet();

        /// <summary>
        /// Gets or set a list url.
        /// </summary>
        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
