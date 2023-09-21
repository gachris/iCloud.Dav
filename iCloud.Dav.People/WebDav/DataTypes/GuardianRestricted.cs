using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Card
{
    /// <summary>
    /// Represents an error 
    /// Represents a guardian-restricted in the context of the <see cref="Prop"/>.
    /// </summary>
    internal class GuardianRestricted : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the value of the guardian-restricted property.
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
        /// Reads the XML representation of the guardian-restricted property.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            if (!reader.IsStartElement("guardian-restricted")) return;

            reader.Read();
            Value = reader.ReadContentAsString();
        }

        /// <summary>
        /// This method is not supported and will throw a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        /// <exception cref="NotSupportedException">This method is not supported.</exception>
        public void WriteXml(XmlWriter writer) => throw new NotSupportedException();

        #endregion
    }
}