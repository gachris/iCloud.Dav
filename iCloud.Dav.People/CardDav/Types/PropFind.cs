using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.CardDav.Types
{
    [XmlRoot(ElementName = "propfind", Namespace = "DAV:")]
    internal sealed class PropFind : IXmlSerializable
    {
        public XmlSchema GetSchema() => new XmlSchema();

        public void ReadXml(XmlReader reader) => throw new NotSupportedException();

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("prop", "DAV:");

            writer.WriteElementString("allprop", "DAV:", null);
            writer.WriteElementString("sync-token", "DAV:", null);

            writer.WriteEndElement();
        }
    }
}