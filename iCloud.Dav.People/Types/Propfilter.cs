using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "prop-filter", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public sealed class Propfilter
    {
        [XmlElement(ElementName = "text-match", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public List<TextMatch> Textmatch { get; set; }

        [XmlElement(ElementName = "is-not-defined")]
        public IsNotDefined Isnotdefined { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
