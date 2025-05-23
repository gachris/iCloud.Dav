﻿using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Auth.WebDav.DataTypes.Cal;

/// <summary>
/// Represents a set of calendar user addresses in the context of CalDAV operations.
/// </summary>
internal class CalendarUserAddressSet : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets an array of hyperlink references (href) representing calendar user addresses.
    /// </summary>
    public Href[] Values { get; set; }

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
        if (!reader.IsStartElement("calendar-user-address-set")) return;

        var hrefs = new List<Href>();

        while (reader.Read())
        {
            if (reader.IsStartElement("href"))
            {
                if (reader.IsEmptyElement) continue;

                var href = new Href();
                href.ReadXml(reader);

                hrefs.Add(href);
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "calendar-user-address-set")
            {
                break;
            }
        }

        Values = hrefs.ToArray();
    }

    /// <summary>
    /// Writes the XML representation of the object.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("calendar-user-address-set", "urn:ietf:params:xml:ns:caldav");
        Values?.ToList().ForEach(value => value.WriteXml(writer));
        writer.WriteEndElement();
    }

    #endregion
}