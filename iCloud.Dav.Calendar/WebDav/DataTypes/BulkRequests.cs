using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.Dav.Core.WebDav.Cal
{
    public class BulkRequests : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets an array of resource type names associated with the property.
        /// </summary>
        public BulkRequest[] Values { get; set; }

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
            if (!reader.IsStartElement("bulk-requests", "http://me.com/_namespace/")) return;

            var bulkRequests = new List<BulkRequest>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    BulkRequest bulkRequest = new BulkRequest();
                    bulkRequest.ReadXml(reader);

                    bulkRequests.Add(bulkRequest);
                }

                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "bulk-requests")
                {
                    break;
                }
            }

            Values = bulkRequests.ToArray();
        }

        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("bulk-requests", "http://me.com/_namespace/", null);
        }

        #endregion
    }

    public class BulkRequest : IXmlSerializable
    {
        #region Properties

        public string LocalName { get; set; }

        public MaxSize MaxSize { get; set; }

        public MaxResources MaxResources { get; set; }

        public Supported Supported { get; set; }

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
            LocalName = reader.LocalName;

            while (reader.Read())
            {
                if (reader.IsStartElement("max-size", "http://me.com/_namespace/") && reader.Depth == 6)
                {
                    MaxSize = new MaxSize();
                    MaxSize.ReadXml(reader);
                }
                else if (reader.IsStartElement("max-resources", "http://me.com/_namespace/") && reader.Depth == 6)
                {
                    MaxResources = new MaxResources();
                    MaxResources.ReadXml(reader);
                }
                else if (reader.IsStartElement("supported", "http://me.com/_namespace/") && reader.Depth == 6)
                {
                    Supported = new Supported();
                    Supported.ReadXml(reader);
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == LocalName && reader.Depth == 5)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
        }

        #endregion
    }

    public class Supported : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets an array of resource type names associated with the property.
        /// </summary>
        public string[] LocalNames { get; set; }

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
            if (!reader.IsStartElement("supported")) return;

            var names = new List<string>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    names.Add(reader.LocalName);
                }

                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "supported")
                {
                    break;
                }
            }

            LocalNames = names.ToArray();
        }

        /// <summary>
        /// Writes the XML representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("supported", "http://me.com/_namespace/");
            LocalNames?.ToList().ForEach(n => writer.WriteElementString(n, "http://me.com/_namespace/", null));
            writer.WriteEndElement();
        }

        #endregion
    }
}