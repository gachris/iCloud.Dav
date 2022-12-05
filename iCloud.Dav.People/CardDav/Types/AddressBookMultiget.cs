using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.CardDav.Types
{
    [XmlRoot(ElementName = "addressbook-multiget", Namespace = "urn:ietf:params:xml:ns:carddav")]
    internal sealed class AddressBookMultiget : IXmlSerializable
    {
        public List<string> Href { get; }

        public AddressBookMultiget()
        {
            Href = new List<string>();
        }

        public XmlSchema GetSchema() => new XmlSchema();

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("prop", "DAV:");
            writer.WriteElementString("allprop", "DAV:", null);
            writer.WriteEndElement();
            Href.ForEach(href => writer.WriteElementString("href", "DAV:", href));
        }
    }
}