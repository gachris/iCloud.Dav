using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.People.CardDav.Types
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

            var me_card = propElements?.FirstOrDefault(x => x.Name.LocalName == "me-card");
            var me_cardhref = me_card?.Elements().FirstOrDefault(x => x.Name.LocalName == "href");
            var me_cardhrefAttribute = !(me_cardhref is null) ? new Attribute(me_cardhref.Name.LocalName,
                                                                               me_cardhref.Name.NamespaceName,
                                                                               me_cardhref.Value) : null;

            var creationdate = propElements?.FirstOrDefault(x => x.Name.LocalName == "creationdate");
            _ = DateTime.TryParse(creationdate?.Value, out var created);

            var getlastmodified = propElements?.FirstOrDefault(x => x.Name.LocalName == "getlastmodified");
            _ = DateTime.TryParse(getlastmodified?.Value, out var modified);

            var guardian_restricted = propElements?.FirstOrDefault(x => x.Name.LocalName == "guardian-restricted");

            var resourcetype = propElements?.FirstOrDefault(x => x.Name.LocalName == "resourcetype")?.Elements()?.
                Select(element => new Attribute(element.Name.LocalName, element.Name.NamespaceName, element.Value)) ?? Enumerable.Empty<Attribute>();

            var getctag = propElements?.FirstOrDefault(x => x.Name.LocalName == "getctag");
            var address_data = propElements?.FirstOrDefault(x => x.Name.LocalName == "address-data");

            response.MeCard = !(me_card is null) ? new Attribute(me_card.Name.LocalName,
                                                                       me_card.Name.NamespaceName,
                                                                       me_card.Value,
                                                                       me_cardhrefAttribute) : null;
            response.GuardianRestricted = !(guardian_restricted is null) ? new Attribute(guardian_restricted.Name.LocalName, guardian_restricted.Name.NamespaceName, guardian_restricted.Value) : null;
            response.Ctag = !(getctag is null) ? new Attribute(getctag.Name.LocalName, getctag.Name.NamespaceName, getctag.Value) : null;
            response.Etag = propElements?.FirstOrDefault(x => x.Name.LocalName == "getetag")?.Value;
            response.AddressData = !(address_data is null) ? new Attribute(address_data.Name.LocalName, address_data.Name.NamespaceName, address_data.Value) : null;
            response.Contentlength = propElements?.FirstOrDefault(x => x.Name.LocalName == "getcontentlength")?.Value;
            response.Modified = modified;
            response.Created = created;
            response.SyncToken = propElements?.FirstOrDefault(x => x.Name.LocalName == "sync-token")?.Value;

            response.ResourceType.AddRange(resourcetype);

            return response;
        }

        public void WriteXml(XmlWriter writer) => throw new NotSupportedException();
    }

    internal sealed class Response
    {
        public string Href { get; set; }

        public Status Status { get; set; }

        public string Contentlength { get; set; }

        public string Etag { get; set; }

        public Attribute Ctag { get; set; }

        public Attribute MeCard { get; set; }

        public Attribute GuardianRestricted { get; set; }

        public Attribute AddressData { get; set; }

        public List<Attribute> ResourceType { get; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string SyncToken { get; set; }

        public Response()
        {
            ResourceType = new List<Attribute>();
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

        public Attribute(string name, string ns, string value, Attribute innerAttribute) : this(name, ns, value) => InnerAttribute = innerAttribute;
    }

    internal enum Status
    {
        OK,
        NotFound
    }
}