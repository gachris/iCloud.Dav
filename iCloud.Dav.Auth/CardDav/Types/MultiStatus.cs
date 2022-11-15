using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Auth.CardDav.Types;

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

    public static Response GetResponse(XElement xResponse)
    {
        var href = xResponse.Elements().First(x => x.Name.LocalName == "href").Value;
        var propstat = xResponse.Elements().First(x => x.Name.LocalName == "propstat");
        var status = propstat.Elements().Single(x => x.Name.LocalName == "status");
        var prop = propstat.Elements().First(x => x.Name.LocalName == "prop");
        var propElements = prop.Elements();

        var current_user_principal = propElements.First(x => x.Name.LocalName == "current-user-principal");
        var current_user_principal_href = current_user_principal.Elements().First(x => x.Name.LocalName == "href").Value.Trim();

        var displayname = propElements?.FirstOrDefault(x => x.Name.LocalName == "displayname")?.Value?.Trim();

        var calendar_home_set = propElements?.FirstOrDefault(x => x.Name.LocalName == "calendar-home-set");
        var calendar_home_set_href = calendar_home_set?.Elements().FirstOrDefault(x => x.Name.LocalName == "href")?.Value?.Trim();

        var addressbook_home_set = propElements?.FirstOrDefault(x => x.Name.LocalName == "addressbook-home-set");
        var addressbook_home_set_href = addressbook_home_set?.Elements().FirstOrDefault(x => x.Name.LocalName == "href")?.Value?.Trim();

        var calendar_user_address_set = propElements?.FirstOrDefault(x => x.Name.LocalName == "calendar-user-address-set");
        var calendar_user_address_set_href = calendar_user_address_set?.Elements().Where(x => x.Name.LocalName == "href");
        var calendar_user_address = calendar_user_address_set_href?.
            Select(element =>
            {
                _ = bool.TryParse(element.Attributes().FirstOrDefault(x => x.Name == "preferred")?.Value, out var preffered);
                return new CalendarUserAddress(preffered, element.Value);
            }) ?? Enumerable.Empty<CalendarUserAddress>().ToList();

        return new(href,
                   current_user_principal_href,
                   calendar_home_set_href,
                   addressbook_home_set_href,
                   displayname,
                   calendar_user_address);
    }

    public void WriteXml(XmlWriter writer) => throw new NotSupportedException();
}
