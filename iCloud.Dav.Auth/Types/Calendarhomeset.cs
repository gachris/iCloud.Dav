using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    /// <summary>
    /// Calendar home set.
    /// </summary>
    [XmlRoot(ElementName = "calendar-home-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class CalendarHomeSet
    {
        /// <summary>
        /// 
        /// </summary>
        public static CalendarHomeSet Default = new CalendarHomeSet();

        /// <summary>
        /// Gets or set a list url.
        /// </summary>
        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
