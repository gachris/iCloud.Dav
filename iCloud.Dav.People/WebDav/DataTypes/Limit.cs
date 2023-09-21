using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Card
{
    /// <summary>
    /// Represents a limit for CardDAV queries in the CardDAV namespace.
    /// </summary>
    internal class Limit : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the number of results to be limited in the query.
        /// </summary>
        public int NResults { get; set; }

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
        /// Writes the XML representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("limit", "urn:ietf:params:xml:ns:carddav");
            writer.WriteElementString("nresults", "urn:ietf:params:xml:ns:carddav", NResults.ToString());
            writer.WriteEndElement();
        }

        #endregion
    }
}