using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Cal
{
    /// <summary>
    /// Represents a collection of supported calendar components in the CalDAV namespace.
    /// </summary>
    public class SupportedCalendarComponentSet : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets an array of <see cref="Comp"/> objects representing the supported calendar components.
        /// </summary>
        public Comp[] Comp { get; set; }

        #endregion

        #region IXmlSerializable Implementation

        /// <summary>
        /// Gets the XML schema for this object.
        /// </summary>
        /// <returns>An <see cref="XmlSchema"/> object.</returns>
        public XmlSchema GetSchema() => new XmlSchema();

        /// <summary>
        /// Reads the XML representation of the supported report set.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            if (!reader.IsStartElement("supported-calendar-component-set")) return;

            var comps = new List<Comp>();

            while (reader.Read())
            {
                if (reader.IsStartElement("comp"))
                {
                    var comp = new Comp();
                    comp.ReadXml(reader);

                    comps.Add(comp);
                }

                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "supported-calendar-component-set")
                {
                    break;
                }
            }

            Comp = comps.ToArray();
        }

        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("supported-calendar-component-set", "urn:ietf:params:xml:ns:caldav");

            Comp?.ToList().ForEach(comp =>
            {
                comp.WriteXml(writer);
            });

            writer.WriteEndElement();
        }

        #endregion
    }
}