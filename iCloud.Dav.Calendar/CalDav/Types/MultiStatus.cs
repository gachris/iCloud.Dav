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

    public MultiStatus()
    {
        Responses = new List<Response>();
    }

    public XmlSchema GetSchema()
    {
        return new XmlSchema();
    }

    public void ReadXml(XmlReader reader)
    {
        var xDocument = XDocument.Load(reader.ReadSubtree());

        var root = (xDocument.FirstNode as XElement).ThrowIfNull(nameof(XElement));
        var rootDescendants = root.Descendants().OfType<XElement>();
        var responses = rootDescendants.Where(x => x.Name.LocalName == "response");

        foreach (var response in responses)
        {
            var href = response.Elements().Where(x => x.Name.LocalName == "href").FirstOrDefault();

            var propstat = response.Elements().Where(x => x.Name.LocalName == "propstat").FirstOrDefault();

            var status = propstat?.Elements().Where(x => x.Name.LocalName == "status").FirstOrDefault();
            var prop = propstat?.Elements().Where(x => x.Name.LocalName == "prop").FirstOrDefault();

            var getctag = prop?.Elements().Where(x => x.Name.LocalName == "getctag").FirstOrDefault();
            var getctagAttribute =
               getctag
               is not null ?
               new Attribute(getctag.Name.LocalName, getctag.Name.NamespaceName, getctag.Value) :
               null;

            var current_user_privilege_set = prop?.Elements().Where(x => x.Name.LocalName == "current-user-privilege-set").FirstOrDefault();
            var privilege = current_user_privilege_set?.Elements().Where(x => x.Name.LocalName == "privilege");
            var privileges = privilege?.Elements()?.
                Select(element => new Privilege(element.Name.LocalName, element.Name.NamespaceName)).
                ToList() ??
                Enumerable.Empty<Privilege>().ToList();

            var supported_report_set = prop?.Elements().Where(x => x.Name.LocalName == "supported-report-set").FirstOrDefault();
            var supported_reports = supported_report_set?.Elements().Where(x => x.Name.LocalName == "supported-report");

            var reports = supported_reports?.Elements()?.
                Select(element =>
                {
                    var report = element.Descendants().FirstOrDefault();
                    return new SupportedReport(report.Name.LocalName, report.Name.NamespaceName);
                }).
                ToList() ??
                Enumerable.Empty<SupportedReport>().ToList();

            var supported_calendar_component_set = prop?.Elements().Where(x => x.Name.LocalName == "supported-calendar-component-set").FirstOrDefault();
            var calendar_component = supported_calendar_component_set?.Elements().Where(x => x.Name.LocalName == "comp");
            var calendar_components = calendar_component?.
                Select(element => new CalendarComponent(element.Attributes().Where(x => x.Name == "name").FirstOrDefault()?.Value, element.Name.NamespaceName)).
                ToList() ??
                Enumerable.Empty<CalendarComponent>().ToList();

            var displayname = prop?.Elements().Where(x => x.Name.LocalName == "displayname").FirstOrDefault();
            var calendar_color = prop?.Elements().Where(x => x.Name.LocalName == "calendar-color").FirstOrDefault();
            var getetag = prop?.Elements().Where(x => x.Name.LocalName == "getetag").FirstOrDefault();

            var calendar_data = prop?.Elements().Where(x => x.Name.LocalName == "calendar-data").FirstOrDefault();
            var calendar_dataAttribute =
                calendar_data
                is not null ?
                new Attribute(calendar_data.Name.LocalName, calendar_data.Name.NamespaceName, calendar_data.Value) :
                null;

            var item = new Response(
                href?.Value,
                status?.Value,
                displayname?.Value,
                calendar_color?.Value,
                getetag?.Value,
                getctagAttribute,
                calendar_dataAttribute,
                privileges,
                reports,
                calendar_components);

            Responses.Add(item);
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new NotSupportedException();
    }
}

internal sealed class Response
{
    public Response(
        string href,
        string status,
        string displayName,
        string calendarColor,
        string etag,
        Attribute ctag,
        Attribute calendarData,
        IReadOnlyList<Privilege> currentUserPrivilegeSet,
        IReadOnlyList<SupportedReport> supportedReportSet,
        IReadOnlyList<CalendarComponent> supportedCalendarComponentSet
        )
    {
        Href = href;
        Status = status;
        DisplayName = displayName;
        CalendarColor = calendarColor;
        Etag = etag;
        Ctag = ctag;
        CalendarData = calendarData;
        CurrentUserPrivilegeSet = currentUserPrivilegeSet;
        SupportedReportSet = supportedReportSet;
        SupportedCalendarComponentSet = supportedCalendarComponentSet;
    }

    public string Href { get; }

    public string Status { get; }

    public string DisplayName { get; }

    public string CalendarColor { get; }

    public string Etag { get; }

    public Attribute Ctag { get; }

    public Attribute CalendarData { get; }

    public IReadOnlyList<Privilege> CurrentUserPrivilegeSet { get; }

    public IReadOnlyList<SupportedReport> SupportedReportSet { get; }

    public IReadOnlyList<CalendarComponent> SupportedCalendarComponentSet { get; }
}

internal sealed class Privilege
{
    public string Name { get; }

    public string NameSpace { get; }

    public Privilege(string name, string nameSpace)
    {
        Name = name;
        NameSpace = nameSpace;
    }
}

internal sealed class SupportedReport
{
    public string Name { get; }

    public string NameSpace { get; }

    public SupportedReport(string name, string nameSpace)
    {
        Name = name;
        NameSpace = nameSpace;
    }
}

internal sealed class CalendarComponent
{
    public string Name { get; }

    public string NameSpace { get; }

    public CalendarComponent(string name, string nameSpace)
    {
        Name = name;
        NameSpace = nameSpace;
    }
}

internal sealed class Attribute
{
    public string Name { get; }

    public string Namespace { get; }

    public string Value { get; }

    public Attribute InnerAttribute { get; }

    public Attribute(string name, string ns, string value)
    {
        Name = name;
        Namespace = ns;
        Value = value;
    }

    public Attribute(string name, string ns, string value, Attribute innerAttribute) : this(name, ns, value)
    {
        InnerAttribute = innerAttribute;
    }
}
