using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
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

        public XmlSchema GetSchema()
        {
            return new XmlSchema();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotSupportedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("prop", "DAV:");

            writer.WriteStartElement("allprop", "DAV:");
            writer.WriteEndElement();

            writer.WriteEndElement();

            if (Filter is not null && Filter.TextMatches.Any())
            {
                writer.WriteStartElement("filter", "urn:ietf:params:xml:ns:carddav");
                writer.WriteAttributeString("test", Filter.Type);

                writer.WriteStartElement("prop-filter", "urn:ietf:params:xml:ns:carddav");
                writer.WriteAttributeString("name", Filter.Name);

                Filter.TextMatches.ForEach<TextMatch>(textMatche =>
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

    internal sealed class Filters
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public IList<TextMatch> TextMatches { get; }

        public Filters()
        {
            TextMatches = new List<TextMatch>();
        }
    }

    internal sealed class TextMatch
    {
        public string Collation { get; set; }
        public string MatchType { get; set; }
        public string SearchText { get; set; }
        public string NegateCondition { get; set; }
    }
}
