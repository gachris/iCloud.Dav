using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.CalDav.Types;

[XmlRoot(ElementName = "multistatus", Namespace = "DAV:")]
internal sealed class MultiStatus : IXmlSerializable
{
    public List<Response> Responses { get; }

    public MultiStatus() => Responses = new();

    public XmlSchema GetSchema() => new();

    public void ReadXml(XmlReader reader)
    {
        var xDocument = XDocument.Load(reader.ReadSubtree());
        var root = (xDocument.FirstNode as XElement).ThrowIfNull(nameof(XElement));
        var rootDescendants = root.Descendants().OfType<XElement>();
        var xResponses = rootDescendants.Where(x => x.Name.LocalName == "response");
        Responses.AddRange(xResponses.Select(GetResponse));
    }

    public void WriteXml(XmlWriter writer) => throw new NotSupportedException();

    private static Response GetResponse(XElement xResponse)
    {
        var href = xResponse.Elements().First(x => x.Name.LocalName == "href").Value;
        var propstat = xResponse.Elements().First(x => x.Name.LocalName == "propstat");
        var status = propstat.Elements().Single(x => x.Name.LocalName == "status");
        var prop = propstat.Elements().FirstOrDefault(x => x.Name.LocalName == "prop");
        var propElements = prop?.Elements();

        var getctag = propElements?.FirstOrDefault(x => x.Name.LocalName == "getctag");
        var getctagAttribute =
           getctag
           is not null ?
           new Attribute(getctag.Name.LocalName, getctag.Name.NamespaceName, getctag.Value) :
           null;

        var current_user_privilege_set = propElements?.FirstOrDefault(x => x.Name.LocalName == "current-user-privilege-set");
        var privilege = current_user_privilege_set?.Elements().Where(x => x.Name.LocalName == "privilege");
        var privileges = privilege?.Elements()?.
            Select(element => new Privilege(element.Name.LocalName, element.Name.NamespaceName)).
            ToList() ??
            Enumerable.Empty<Privilege>().ToList();

        var supported_report_set = propElements?.FirstOrDefault(x => x.Name.LocalName == "supported-report-set");
        var supported_reports = supported_report_set?.Elements().Where(x => x.Name.LocalName == "supported-report");

        var reports = supported_reports?.Elements()?.
            Select(element =>
            {
                var report = element.Descendants().First();
                return new SupportedReport(report.Name.LocalName, report.Name.NamespaceName);
            }).
            ToList() ??
            Enumerable.Empty<SupportedReport>().ToList();

        var supported_calendar_component_set = propElements?.FirstOrDefault(x => x.Name.LocalName == "supported-calendar-component-set");
        var calendar_component = supported_calendar_component_set?.Elements().Where(x => x.Name.LocalName == "comp");
        var calendar_components = calendar_component?.
            Select(element => new CalendarComponent(element.Attributes().First(x => x.Name == "name").Value, element.Name.NamespaceName)).
            ToList() ??
            Enumerable.Empty<CalendarComponent>().ToList();

        var displayname = propElements?.FirstOrDefault(x => x.Name.LocalName == "displayname")?.Value;
        var calendar_color = propElements?.FirstOrDefault(x => x.Name.LocalName == "calendar-color")?.Value;
        var getetag = propElements?.FirstOrDefault(x => x.Name.LocalName == "getetag")?.Value;

        var calendar_data = propElements?.FirstOrDefault(x => x.Name.LocalName == "calendar-data");
        var calendar_dataAttribute = calendar_data is not null ? new Attribute(calendar_data.Name.LocalName,
                                                                               calendar_data.Name.NamespaceName,
                                                                               calendar_data.Value) : null;

        var syncToken = propElements?.FirstOrDefault(x => x.Name.LocalName == "sync-token")?.Value;

        return new(href,
                   displayname,
                   calendar_color,
                   getetag,
                   getctagAttribute,
                   calendar_dataAttribute,
                   privileges,
                   reports,
                   calendar_components,
                   syncToken);
    }
}