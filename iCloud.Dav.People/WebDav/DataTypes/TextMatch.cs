using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Card
{
    /// <summary>
    /// Represents a text match condition used in CardDAV queries.
    /// </summary>
    internal class TextMatch : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the collation used for the text match condition.
        /// </summary>
        public string Collation { get; set; }

        /// <summary>
        /// Gets or sets the type of the text match.
        /// </summary>
        public string MatchType { get; set; }

        /// <summary>
        /// Gets or sets the search text for the text match condition.
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether to negate the text match condition.
        /// </summary>
        public string NegateCondition { get; set; }

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
        /// Writes the XML representation of the text match condition.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("text-match", "urn:ietf:params:xml:ns:carddav");
            writer.WriteAttributeString("collation", Collation);
            writer.WriteAttributeString("negate-condition", NegateCondition);
            writer.WriteAttributeString("match-type", MatchType);
            writer.WriteString(SearchText);
            writer.WriteEndElement();
        }

        #endregion
    }
}