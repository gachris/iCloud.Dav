using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "filter", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public class Filters
    {
        [XmlElement(ElementName = "prop-filter", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public List<Propfilter> Propfilter { get; set; }

        [XmlAttribute(AttributeName = "test")]
        public string Type { get; set; }
    }
}
