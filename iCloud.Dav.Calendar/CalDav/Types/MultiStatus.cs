using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Calendar.CalDav.Types
{
    [XmlRoot(ElementName = "multistatus", Namespace = "DAV:")]
    internal sealed class MultiStatus : IXmlSerializable
    {
        public string SyncToken { get; private set; }

        public List<Response> Responses { get; }

        public MultiStatus() => Responses = new List<Response>();

        public XmlSchema GetSchema() => new XmlSchema();

        public void ReadXml(XmlReader reader)
        {
            var xDocument = XDocument.Load(reader.ReadSubtree());
            var root = (xDocument.FirstNode as XElement).ThrowIfNull(nameof(XElement));
            var rootDescendants = root.Descendants().OfType<XElement>();
            var xResponses = rootDescendants.Where(x => x.Name.LocalName == "response");
            Responses.AddRange(xResponses.Select(GetResponse));
            SyncToken = rootDescendants.FirstOrDefault(x => x.Name.LocalName == "sync-token")?.Value;
        }

        public void WriteXml(XmlWriter writer) => throw new NotSupportedException();

        private static Response GetResponse(XElement xResponse)
        {
            var response = new Response
            {
                Href = xResponse.Elements().First(x => x.Name.LocalName == "href").Value,
                Status = xResponse.Elements().FirstOrDefault(x => x.Name.LocalName == "status")?.Value == "HTTP/1.1 404 Not Found" ? Status.NotFound : Status.OK
            };

            if (response.Status == Status.NotFound)
            {
                return response;
            }

            var propstat = xResponse.Elements().First(x => x.Name.LocalName == "propstat");
            var prop = propstat.Elements().FirstOrDefault(x => x.Name.LocalName == "prop");
            var propElements = prop?.Elements();
            var privilege = propElements?.FirstOrDefault(x => x.Name.LocalName == "current-user-privilege-set")?.Elements().Where(x => x.Name.LocalName == "privilege");
            var privileges = privilege?.Elements()?.
                Select(element => new Privilege(element.Name.LocalName, element.Name.NamespaceName)) ?? Enumerable.Empty<Privilege>();
            var supported_reports = propElements?.FirstOrDefault(x => x.Name.LocalName == "supported-report-set")?.Elements().Where(x => x.Name.LocalName == "supported-report");
            var reports = supported_reports?.Elements()?.
                Select(element =>
                {
                    var report = element.Descendants().First();
                    return new SupportedReport(report.Name.LocalName, report.Name.NamespaceName);
                }) ?? Enumerable.Empty<SupportedReport>();
            var calendar_component = propElements?.FirstOrDefault(x => x.Name.LocalName == "supported-calendar-component-set")?.Elements().Where(x => x.Name.LocalName == "comp");
            var calendar_components = calendar_component?.
                Select(element => new CalendarComponent(element.Attributes().First(x => x.Name == "name").Value, element.Name.NamespaceName)) ?? Enumerable.Empty<CalendarComponent>();
            var resourcetype = propElements?.FirstOrDefault(x => x.Name.LocalName == "resourcetype")?.Elements()?.
                Select(element => new ResourceType(element.Name.LocalName, element.Name.NamespaceName)) ?? Enumerable.Empty<ResourceType>();
            var calendar_data = propElements?.FirstOrDefault(x => x.Name.LocalName == "calendar-data");
            var getctag = propElements?.FirstOrDefault(x => x.Name.LocalName == "getctag");

            response.Ctag = !(getctag is null) ? new Attribute(getctag.Name.LocalName, getctag.Name.NamespaceName, getctag.Value) : null;
            response.DisplayName = propElements?.FirstOrDefault(x => x.Name.LocalName == "displayname")?.Value;
            response.CalendarColor = propElements?.FirstOrDefault(x => x.Name.LocalName == "calendar-color")?.Value;
            response.CalendarOrder = propElements?.FirstOrDefault(x => x.Name.LocalName == "calendar-order")?.Value;
            response.Etag = propElements?.FirstOrDefault(x => x.Name.LocalName == "getetag")?.Value;
            response.SyncToken = propElements?.FirstOrDefault(x => x.Name.LocalName == "sync-token")?.Value;
            response.CalendarTimeZone = propElements?.FirstOrDefault(x => x.Name.LocalName == "calendar-timezone")?.Value;
            response.CalendarDescription = propElements?.FirstOrDefault(x => x.Name.LocalName == "calendar-description")?.Value;
            response.CalendarData = !(calendar_data is null) ? new Attribute(calendar_data.Name.LocalName,
                                                                                   calendar_data.Name.NamespaceName,
                                                                                   calendar_data.Value) : null;

            response.CurrentUserPrivilegeSet.AddRange(privileges);
            response.SupportedReportSet.AddRange(reports);
            response.SupportedCalendarComponentSet.AddRange(calendar_components);
            response.ResourceType.AddRange(resourcetype);

            return response;
        }
    }
}