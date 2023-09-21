using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Cal
{
    /// <summary>
    /// Represents a resource type used in CalDAV queries in the CalDAV namespace.
    /// </summary>
    public class Privilege : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets privilege associated with the property.
        /// </summary>
        public string Name { get; set; }

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
            if (!reader.IsStartElement("privilege")) return;

            reader.Read();
            Name = reader.LocalName;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "privilege")
                {
                    break;
                }
            }
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