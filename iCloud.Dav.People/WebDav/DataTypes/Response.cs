using iCloud.Dav.People.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.WebDav.DataTypes;

/// <summary>
/// Represents a response in the <see cref="MultiStatus"/> response.
/// </summary>
internal class Response : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets the href of the response.
    /// </summary>
    public Href Href { get; set; }

    /// <summary>
    /// Gets or sets an array of <see cref="PropStat"/> objects representing the property status in the response.
    /// </summary>
    public PropStat[] PropStat { get; set; }

    /// <summary>
    /// Gets or sets the status of the response.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Card.Error"/> object representing the error in the response.
    /// </summary>
    public Error Error { get; set; }

    /// <summary>
    /// Gets the <see cref="HttpStatusCode"/> value parsed from the response status.
    /// </summary>
    /// <remarks>
    /// This property converts the status string to an equivalent <see cref="HttpStatusCode"/> value.
    /// If the conversion fails, it returns null.
    /// </remarks>
    public HttpStatusCode? StatusCode => Status.ToHttpStatusCode();

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
        if (!reader.IsStartElement("response")) return;

        var propStatList = new List<PropStat>();

        while (reader.Read())
        {
            if (reader.IsStartElement("href"))
            {
                Href = new Href();
                Href.ReadXml(reader);
            }
            else if (reader.IsStartElement("propstat"))
            {
                var propStat = new PropStat();
                propStat.ReadXml(reader);
                propStatList.Add(propStat);
            }
            else if (reader.IsStartElement("status"))
            {
                reader.Read();
                Status = reader.ReadContentAsString();
            }
            else if (reader.IsStartElement("error"))
            {
                Error = new Error();
                Error.ReadXml(reader);
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "response")
            {
                break;
            }
        }

        PropStat = propStatList.ToArray();
    }

    /// <summary>
    /// This method is not supported and will throw a <see cref="NotSupportedException"/>.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    /// <exception cref="NotSupportedException">This method is not supported.</exception>
    public void WriteXml(XmlWriter writer) => throw new NotSupportedException();

    #endregion
}