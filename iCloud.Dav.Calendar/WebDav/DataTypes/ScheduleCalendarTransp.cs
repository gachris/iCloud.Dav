using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Cal
{
    internal class ScheduleCalendarTransp : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets an array of names associated with the property.
        /// </summary>
        public string[] Names { get; set; }

        #endregion

        #region IXmlSerializable Implementation

        /// <summary>
        /// Gets the XML schema for this object.
        /// </summary>
        /// <returns>An <see cref="XmlSchema"/> object.</returns>
        public XmlSchema GetSchema() => new XmlSchema();

        /// <summary>
        /// Reads the XML representation of the resource type.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            if (!reader.IsStartElement("schedule-calendar-transp")) return;

            var names = new List<string>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    names.Add(reader.LocalName);
                }

                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "schedule-calendar-transp")
                {
                    break;
                }
            }

            Names = names.ToArray();
        }


        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("schedule-calendar-transp", "urn:ietf:params:xml:ns:caldav", null);
        }

        #endregion
    }
}