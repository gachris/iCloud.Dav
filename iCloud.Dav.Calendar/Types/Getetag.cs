using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.Types
{
    [XmlRoot(ElementName = "getetag", Namespace = "DAV:")]
    public class Getetag
    {
        public static Getetag Default = new Getetag();

        [XmlText]
        public string Value { get; set; }
    }
}
