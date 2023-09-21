using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Cal
{
    public class ScheduleDefaultCalendarURL : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the value of the schedule-default-calendar-URL.
        /// </summary>
        public string Value { get; set; }

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
            if (!reader.IsStartElement("schedule-default-calendar-URL")) return;

            reader.Read();
            Value = reader.ReadContentAsString();
        }

        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("schedule-default-calendar-URL", "urn:ietf:params:xml:ns:caldav", null);
        }

        #endregion
    }
}