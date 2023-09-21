using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Cal
{
    /// <summary>
    /// Represents a time range in the CalDAV namespace.
    /// </summary>
    internal class TimeRange : IXmlSerializable, IFilter
    {
        #region Properties

        /// <summary>
        /// This property is not supported and will throw a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">This property is not supported.</exception>
        public IEnumerable<IFilter> Children => throw new NotSupportedException();

        /// <summary>
        /// Gets or sets the start time of the time range.
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// Gets or sets the end time of the time range.
        /// </summary>
        public string End { get; set; }

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
            writer.WriteStartElement("time-range", "urn:ietf:params:xml:ns:caldav");
            writer.WriteAttributeString("start", Start);
            writer.WriteAttributeString("end", End);
            writer.WriteEndElement();
        }

        #endregion
    }
}