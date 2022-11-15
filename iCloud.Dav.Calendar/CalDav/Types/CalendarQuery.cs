using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.CalDav.Types;

[XmlRoot(ElementName = "calendar-query", Namespace = "urn:ietf:params:xml:ns:caldav")]
internal sealed class CalendarQuery : IXmlSerializable
{
    public CompFilter? CompFilter { get; set; }

    public XmlSchema GetSchema() => new XmlSchema();

    public void ReadXml(XmlReader reader) => throw new NotSupportedException();

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("prop", "DAV:");
        writer.WriteElementString("calendar-data", "urn:ietf:params:xml:ns:caldav", null);
        writer.WriteElementString("getetag", "DAV:", null);
        writer.WriteEndElement();

        if (CompFilter is not null)
        {
            writer.WriteStartElement("filter", "urn:ietf:params:xml:ns:caldav");
            WriteCompFilterXml(writer, CompFilter);
            writer.WriteEndElement();
        }
    }

    private static void WriteCompFilterXml(XmlWriter writer, CompFilter compFilter)
    {
        writer.WriteStartElement("comp-filter", "urn:ietf:params:xml:ns:caldav");
        writer.WriteAttributeString("name", compFilter.Name);

        if (compFilter.TimeRange is not null)
        {
            writer.WriteStartElement("time-range", "urn:ietf:params:xml:ns:caldav");
            writer.WriteAttributeString("start", compFilter.TimeRange.Start);
            writer.WriteAttributeString("end", compFilter.TimeRange.End);
            writer.WriteEndElement();
        }

        if (compFilter.Child is not null) WriteCompFilterXml(writer, compFilter.Child);

        writer.WriteEndElement();
    }
}