﻿using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.WebDav.DataTypes;

internal class SupportedCalendarComponentSets : IXmlSerializable
{
    #region Properties

    public SupportedCalendarComponentSet[] SupportedCalendarComponentSet { get; set; }

    #endregion

    #region IXmlSerializable Implementation

    /// <summary>
    /// Gets the XML schema for this object.
    /// </summary>
    /// <returns>An <see cref="XmlSchema"/> object.</returns>
    public XmlSchema GetSchema() => new XmlSchema();

    /// <summary>
    /// Reads the XML representation of the multi-status response.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
    public void ReadXml(XmlReader reader)
    {
        if (!reader.IsStartElement("supported-calendar-component-sets")) return;

        var supportedCalendarComponentSets = new List<SupportedCalendarComponentSet>();

        while (reader.Read())
        {
            if (reader.IsStartElement("supported-calendar-component-set"))
            {
                if (reader.IsEmptyElement) continue;

                var supportedCalendarComponentSet = new SupportedCalendarComponentSet();
                supportedCalendarComponentSet.ReadXml(reader);
                supportedCalendarComponentSets.Add(supportedCalendarComponentSet);
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "supported-calendar-component-sets")
            {
                break;
            }
        }

        SupportedCalendarComponentSet = supportedCalendarComponentSets.ToArray();
    }

    /// <summary>
    /// Writes the XML representation of the property update.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("supported-calendar-component-sets", "urn:ietf:params:xml:ns:caldav");
        SupportedCalendarComponentSet?.ToList().ForEach(s => s.WriteXml(writer));
        writer.WriteEndElement();
    }

    #endregion
}