﻿using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.WebDav.DataTypes;

/// <summary>
/// Represents a resource type used in CardDAV queries in the CardDAV namespace.
/// </summary>
internal class ResourceType : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets an array of resource type names associated with the property.
    /// </summary>
    public string[] Names { get; set; }

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
        if (!reader.IsStartElement("resourcetype")) return;

        var names = new List<string>();

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                names.Add(reader.LocalName);
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "resourcetype")
            {
                break;
            }
        }

        Names = names.ToArray();
    }

    /// <summary>
    /// This method is not supported and will throw a <see cref="NotSupportedException"/>.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    /// <exception cref="NotSupportedException">This method is not supported.</exception>
    public void WriteXml(XmlWriter writer) => throw new NotSupportedException();

    #endregion
}