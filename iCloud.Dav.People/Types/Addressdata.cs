using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Dav.People.Types
{
    [XmlRoot(ElementName = "address-data", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public sealed class Addressdata
    {
        [XmlElement(ElementName = "prop", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public List<Properties> Prop { get; set; }

        [XmlText]
        public string Value { get; set; }

        [XmlRoot(ElementName = "prop", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public class Properties
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }
    }
}
