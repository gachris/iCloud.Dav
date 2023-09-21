using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml;

namespace iCloud.Dav.Core.WebDav.Cal
{
    internal class MaxResources : IXmlSerializable
    {
        #region Properties

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
            if (!reader.IsStartElement("max-resources", "http://me.com/_namespace/")) return;
            if (reader.IsEmptyElement) return;

            reader.Read();
            Value = reader.ReadContentAsString();
        }

        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("max-resources", "http://me.com/_namespace/", Value);
        }

        #endregion
    }
}