using System.Xml.Serialization;

namespace iCloud.Dav.Auth.Types
{
    /// <summary>
    /// Display name.
    /// </summary>
    [XmlRoot(ElementName = "displayname", Namespace = "DAV:")]
    public class DisplayName
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DisplayName Default = new DisplayName();

        /// <summary>
        /// Gets or set a value.
        /// </summary>
        [XmlText]
        public string Value { get; set; }
    }
}
