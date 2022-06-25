using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "getetag", Namespace = "DAV:")]
    public sealed class Getetag
    {
        internal static Getetag Default = new Getetag();

        [XmlText]
        public string Value { get; set; }
    }
}
