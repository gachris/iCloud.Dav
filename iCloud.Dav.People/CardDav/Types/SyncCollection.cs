using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.CardDav.Types
{
    [XmlRoot(ElementName = "sync-collection", Namespace = "DAV:")]
    internal sealed class SyncCollection : IXmlSerializable
    {
        public SyncCollection()
        {
            SyncLevel = "1";
        }

        public string SyncToken { get; set; }

        public string SyncLevel { get; set; }

        public XmlSchema GetSchema() => new XmlSchema();

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("sync-token", "DAV:", SyncToken);
            writer.WriteElementString("sync-level", "DAV:", SyncLevel);
            writer.WriteStartElement("prop", "DAV:");
            writer.WriteElementString("getetag", "DAV:", null);
            writer.WriteEndElement();
        }
    }
}