using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Cal
{
    /// <summary>
    /// Represents the pushkey associated with a prop.
    /// </summary>
    internal class QuotaAvailableBytes : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the value of this quota-available-bytes.
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
        /// Reads the XML representation of the hyperlink reference.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            if (!reader.IsStartElement("quota-available-bytes")) return;

            reader.Read();
            Value = reader.ReadContentAsString();
        }

        /// <summary>
        /// Writes the XML representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("quota-available-bytes", "DAV:", Value);
        }

        #endregion
    }
}