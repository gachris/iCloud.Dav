﻿using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.WebDav.DataTypes;

/// <summary>
/// Represents a calendar query in the CalDAV namespace.
/// </summary>
[XmlRoot(ElementName = "calendar-query", Namespace = "urn:ietf:params:xml:ns:caldav")]
internal class CalendarQuery : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets the prop of the calendar-multiget request.
    /// </summary>
    public Prop Prop { get; set; }

    /// <summary>
    /// Gets or sets the filter used in the calendar query.
    /// </summary>
    public Filter Filter { get; set; }

    #endregion

    #region IXmlSerializable Implementation

    /// <summary>
    /// Gets the XML schema for this object.
    /// </summary>
    /// <returns>An <see cref="XmlSchema"/> object.</returns>
    public XmlSchema GetSchema() => new XmlSchema();

    /// <summary>
    /// This method is not supported and will throw a <see cref="NotSupportedException"/>.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
    /// <exception cref="NotSupportedException">This method is not supported.</exception>
    public void ReadXml(XmlReader reader) => throw new NotSupportedException();

    /// <summary>
    /// Writes the XML representation of the property update.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        Prop?.WriteXml(writer);
        Filter?.WriteXml(writer);
    }

    #endregion
}