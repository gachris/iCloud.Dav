using iCloud.Dav.Core.Utils;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.CardDav.Types
{
    [XmlRoot(ElementName = "addressbook-query", Namespace = "urn:ietf:params:xml:ns:carddav")]
    internal sealed class AddressBookQuery : IXmlSerializable
    {
        public Filters Filter { get; set; }

        public XmlSchema GetSchema() => new XmlSchema();

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("prop", "DAV:");
            writer.WriteElementString("allprop", "DAV:", null);
            writer.WriteEndElement();

            if (!(Filter is null) && Filter.TextMatches.Any())
            {
                writer.WriteStartElement("filter", "urn:ietf:params:xml:ns:carddav");
                writer.WriteAttributeString("test", Filter.Type);

                writer.WriteStartElement("prop-filter", "urn:ietf:params:xml:ns:carddav");
                writer.WriteAttributeString("name", Filter.Name);

                if (Filter.IsNotDefined)
                {
                    writer.WriteStartElement("is-not-defined", "urn:ietf:params:xml:ns:carddav");
                    writer.WriteEndElement();
                }

                Filter.TextMatches.ForEach(textMatche =>
                {
                    writer.WriteStartElement("text-match", "urn:ietf:params:xml:ns:carddav");
                    writer.WriteAttributeString("collation", textMatche.Collation);
                    writer.WriteAttributeString("negate-condition", textMatche.NegateCondition);
                    writer.WriteAttributeString("match-type", textMatche.MatchType);
                    writer.WriteString(textMatche.SearchText);
                    writer.WriteEndElement();
                });

                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
    }
}