using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.WebDav.DataTypes;

/// <summary>
/// Represents a property filter in the CardDAV namespace.
/// </summary>
internal class PropFilter : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets the name attribute of the prop-filter.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether the filter is not defined.
    /// </summary>
    public bool IsNotDefined { get; set; }

    /// <summary>
    /// Gets or sets an array of <see cref="TextMatch"/> objects representing the text match conditions in the filter.
    /// </summary>
    public TextMatch[] TextMatches { get; set; }

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
    /// Writes the XML representation of the filter.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("prop-filter", "urn:ietf:params:xml:ns:carddav");
        writer.WriteAttributeString("name", Name);

        if (IsNotDefined)
        {
            writer.WriteStartElement("is-not-defined", "urn:ietf:params:xml:ns:carddav");
            writer.WriteEndElement();
        }

        if (TextMatches != null)
        {
            foreach (var textMatch in TextMatches)
            {
                textMatch.WriteXml(writer);
            }
        }

        writer.WriteEndElement();
    }

    #endregion
}