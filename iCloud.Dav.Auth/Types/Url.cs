using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    /// <summary>
    /// Url.
    /// </summary>
    [XmlRoot(ElementName = "href", Namespace = "DAV:")]
    public class Url
    {
        /// <summary>
        /// Gets or set a value.
        /// </summary>
        [XmlText]
        public string Value { get; set; }

        /// <summary>
        /// Gets or set a preferred.
        /// </summary>
        [XmlAttribute(AttributeName = "preferred")]
        public string Preferred { get; set; }
    }
}
