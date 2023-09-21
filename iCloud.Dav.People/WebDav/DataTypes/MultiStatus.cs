using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Core.WebDav.Card
{
    /// <summary>
    /// Represents a multi-status response in the DAV: namespace. This class is used to deserialize the response from
    /// a WebDAV server.
    /// </summary>
    [XmlRoot(ElementName = "multistatus", Namespace = "DAV:")]
    internal class MultiStatus : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets a list of <see cref="Response"/> objects representing the responses in this multi-status response.
        /// </summary>
        public Response[] Responses { get; set; }

        /// <summary>
        /// Gets or sets a sync token representing the sync token in this multi-status response.
        /// </summary>
        public SyncToken SyncToken { get; set; }

        #endregion

        #region IXmlSerializable Implementation

        /// <summary>
        /// Gets the XML schema for this object.
        /// </summary>
        /// <returns>An <see cref="XmlSchema"/> object.</returns>
        public XmlSchema GetSchema() => new XmlSchema();

        /// <summary>
        /// Reads the XML representation of the multi-status response.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
        public void ReadXml(XmlReader reader)
        {
            if (!reader.IsStartElement("multistatus")) return;

            var responsesList = new List<Response>();

            while (reader.Read())
            {
                if (reader.IsStartElement("response", "DAV:"))
                {
                    var response = new Response();
                    response.ReadXml(reader);
                    responsesList.Add(response);
                }
                else if (reader.IsStartElement("sync-token", "DAV:"))
                {
                    SyncToken = new SyncToken();
                    SyncToken.ReadXml(reader);
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "multistatus")
                {
                    break;
                }
            }

            Responses = responsesList.ToArray();

            reader.Close();
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