using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Card
{
    /// <summary>
    /// Represents a sync collection in the DAV namespace.
    /// </summary>
    [XmlRoot(ElementName = "sync-collection", Namespace = "DAV:")]
    public class SyncCollection : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the sync token of the sync-collection.
        /// </summary>
        public SyncToken SyncToken { get; set; }

        /// <summary>
        /// Gets or sets the sync level of the sync-collection.
        /// </summary>
        public SyncLevel SyncLevel { get; set; }

        /// <summary>
        /// Gets or sets the prop of the sync-collection request.
        /// </summary>
        public Prop Prop { get; set; }

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
            SyncToken?.WriteXml(writer);
            SyncLevel?.WriteXml(writer);
            Prop?.WriteXml(writer);
        }

        #endregion
    }
}