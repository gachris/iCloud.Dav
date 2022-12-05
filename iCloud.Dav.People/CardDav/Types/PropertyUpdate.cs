using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.CardDav.Types
{
    [XmlRoot(ElementName = "propertyupdate", Namespace = "DAV:")]
    internal sealed class PropertyUpdate : IXmlSerializable
    {
        public string Href { get; }

        public PropertyUpdate(string href)
        {
            Href = href;
        }

        public XmlSchema GetSchema() => new XmlSchema();

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("set", "DAV:");
            writer.WriteStartElement("prop", "DAV:");
            writer.WriteStartElement("me-card", "http://calendarserver.org/ns/");
            writer.WriteElementString("href", "DAV:", Href);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}