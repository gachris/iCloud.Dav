using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    /// <summary>
    /// Auth properties.
    /// </summary>
    [XmlRoot(ElementName = "prop", Namespace = "DAV:")]
    public class Prop
    {
        /// <summary>
        /// Gets or set a current user principal.
        /// </summary>
        [XmlElement(ElementName = "current-user-principal", Namespace = "DAV:")]
        public CurrentUserPrincipal CurrentUserPrincipal { get; set; }

        /// <summary>
        /// Gets or set a calendar home set.
        /// </summary>
        [XmlElement(ElementName = "calendar-home-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public CalendarHomeSet CalendarHomeSet { get; set; }

        /// <summary>
        /// Gets or set a display name.
        /// </summary>
        [XmlElement(ElementName = "displayname", Namespace = "DAV:")]
        public DisplayName DisplayName { get; set; }

        /// <summary>
        /// Gets or set a addressbook home set.
        /// </summary>
        [XmlElement(ElementName = "addressbook-home-set", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public AddressBookHomeSet AddressBookHomeSet { get; set; }

        /// <summary>
        /// Gets or set a calendar user address set.
        /// </summary>
        [XmlElement(ElementName = "calendar-user-address-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public CalendarUserAddressSet CalendarUserAddressSet { get; set; }
    }
}
