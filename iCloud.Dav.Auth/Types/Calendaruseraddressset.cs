using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    /// <summary>
    /// Calendar user address set.
    /// </summary>
    [XmlRoot(ElementName = "calendar-user-address-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class CalendarUserAddressSet
    {
        /// <summary>
        /// 
        /// </summary>
        public static CalendarUserAddressSet Default = new CalendarUserAddressSet();

        /// <summary>
        /// Gets or set a list url.
        /// </summary>
        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public List<Url> Href { get; set; }
    }
}
