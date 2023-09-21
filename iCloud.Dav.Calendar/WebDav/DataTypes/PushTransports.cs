using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.Dav.Core.WebDav.Cal
{
    internal class PushTransports : IXmlSerializable
    {
        #region Properties

        public Transport[] Transports { get; set; }

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
        public void ReadXml(XmlReader reader)
        {
            if (!reader.IsStartElement("push-transports")) return;

            var transportsCollection = new List<Transport>();

            while (reader.Read())
            {
                if (reader.IsStartElement("transport"))
                {
                    if (reader.IsEmptyElement) continue;

                    var transports = new Transport();
                    transports.ReadXml(reader);
                    transportsCollection.Add(transports);
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "push-transports")
                {
                    break;
                }
            }

            Transports = transportsCollection.ToArray();
        }

        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("push-transports", "http://calendarserver.org/ns/");
            Transports?.ToList().ForEach(t => t.WriteXml(writer));
            writer.WriteEndElement();
        }

        #endregion
    }
}