using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml;
using System;

namespace iCloud.Dav.Core.WebDav.Cal
{
    internal class Autoprovisioned : IXmlSerializable
    {
        #region Properties

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
            writer.WriteElementString("autoprovisioned", "http://apple.com/ns/ical/", null);
        }

        #endregion
    }
}