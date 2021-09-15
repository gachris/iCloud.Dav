using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    /// <summary>
    /// Current user principal.
    /// </summary>
    [XmlRoot(ElementName = "current-user-principal", Namespace = "DAV:")]
    public class CurrentUserPrincipal
    {
        /// <summary>
        /// 
        /// </summary>
        public static CurrentUserPrincipal Default = new CurrentUserPrincipal();

        /// <summary>
        /// Gets or set a url.
        /// </summary>
        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
