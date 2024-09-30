using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.WebDav.DataTypes;

/// <summary>
/// Represents a supported report in the DAV namespace.
/// </summary>
internal class SupportedReport : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets the <see cref="Report"/> object representing the supported report.
    /// </summary>
    public Report Report { get; set; }

    #endregion

    #region IXmlSerializable Implementation

    /// <summary>
    /// Gets the XML schema for this object.
    /// </summary>
    /// <returns>An <see cref="XmlSchema"/> object.</returns>
    public XmlSchema GetSchema() => new XmlSchema();

    /// <summary>
    /// Reads the XML representation of the supported report.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
    public void ReadXml(XmlReader reader)
    {
        if (!reader.IsStartElement("supported-report")) return;

        while (reader.Read())
        {
            if (reader.IsStartElement("report"))
            {
                if (reader.IsEmptyElement) continue;

                Report = new Report();
                Report.ReadXml(reader);
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "supported-report")
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