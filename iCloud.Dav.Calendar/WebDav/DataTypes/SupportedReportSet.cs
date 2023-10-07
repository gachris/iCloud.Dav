using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.WebDav.DataTypes;

/// <summary>
/// Represents a collection of supported reports in the DAV namespace.
/// </summary>
internal class SupportedReportSet : IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Gets or sets an array of <see cref="SupportedReport"/> objects representing the supported reports.
    /// </summary>
    public SupportedReport[] SupportedReport { get; set; }

    #endregion

    #region IXmlSerializable Implementation

    /// <summary>
    /// Gets the XML schema for this object.
    /// </summary>
    /// <returns>An <see cref="XmlSchema"/> object.</returns>
    public XmlSchema GetSchema() => new XmlSchema();

    /// <summary>
    /// Reads the XML representation of the supported report set.
    /// </summary>
    /// <param name="reader">The <see cref="XmlReader"/> object to read from.</param>
    public void ReadXml(XmlReader reader)
    {
        if (!reader.IsStartElement("supported-report-set")) return;

        var supportedReports = new List<SupportedReport>();

        while (reader.Read())
        {
            if (reader.IsStartElement("supported-report"))
            {
                if (reader.IsEmptyElement) continue;

                var supportedReport = new SupportedReport();
                supportedReport.ReadXml(reader);

                supportedReports.Add(supportedReport);
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "supported-report-set")
            {
                break;
            }
        }

        SupportedReport = supportedReports.ToArray();
    }

    /// <summary>
    /// Writes the XML representation of the property update.
    /// </summary>
    /// <param name="writer">The <see cref="XmlWriter"/> object to write to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("supported-report-set", "DAV:", null);
    }

    #endregion
}