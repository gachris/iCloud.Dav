﻿using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Cal
{
    /// <summary>
    /// Represents a calendar component in the CalDAV namespace.
    /// </summary>
    internal class Comp : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the calendar component.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region IXmlSerializable Implementation

        /// <summary>
        /// Gets the XML schema for this object.
        /// </summary>
        /// <returns>An <see cref="XmlSchema"/> object.</returns>
        public XmlSchema GetSchema() => new XmlSchema();

        /// <summary>
        /// Reads the XML representation of the report.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            if (!reader.IsStartElement("comp")) return;

            Name = reader.GetAttribute("name");
        }

        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("comp", "urn:ietf:params:xml:ns:caldav");
            writer.WriteAttributeString("name", Name);
            writer.WriteEndElement();
        }

        #endregion
    }
}