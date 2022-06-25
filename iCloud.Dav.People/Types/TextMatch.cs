using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "text-match", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public sealed class TextMatch
    {
        [XmlAttribute(AttributeName = "collation")]
        public string Collation { get; set; }

        [XmlAttribute(AttributeName = "match-type")]
        public string Matchtype { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "negate-condition")]
        public string Negatecondition { get; set; }
    }
}
