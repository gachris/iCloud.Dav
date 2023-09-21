using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Schema;
using System;

namespace iCloud.Dav.Core.WebDav.Cal
{
    /// <summary>
    /// Represents the current user's privilege set in the CalDAV namespace.
    /// </summary>
    public class CurrentUserPrivilegeSet : IXmlSerializable
    {
        #region Properties

        /// <summary>
        /// Gets or sets an array of <see cref="Cal.Privilege"/> objects representing the privileges associated with the property.
        /// </summary>
        public Privilege[] Privilege { get; set; }

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
            if (!reader.IsStartElement("current-user-privilege-set")) return;

            var privileges = new List<Privilege>();

            while (reader.Read())
            {
                if (reader.IsStartElement("privilege"))
                {
                    if (reader.IsEmptyElement) continue;

                    var privilege = new Privilege();
                    privilege.ReadXml(reader);

                    privileges.Add(privilege);
                }

                if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "current-user-privilege-set")
                {
                    break;
                }
            }

            Privilege = privileges.ToArray();
        }

        /// <summary>
        /// Writes the XML representation of the property update.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("current-user-privilege-set", "DAV:", null);
        }

        #endregion
    }
}