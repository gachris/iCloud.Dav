using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

                var me_card = prop?.Elements().Where(x => x.Name.LocalName == "me-card").FirstOrDefault();
                var me_cardhref = me_card?.Elements().Where(x => x.Name.LocalName == "href").FirstOrDefault();
                var me_cardhrefAttribute =
                  me_cardhref
                  is not null ?
                  new Attribute(me_cardhref.Name.LocalName, me_cardhref.Name.NamespaceName, me_cardhref.Value) :
                  null;

                var me_cardAttribute =
                 me_card
                 is not null ?
                 new Attribute(me_card.Name.LocalName, me_card.Name.NamespaceName, me_card.Value, me_cardhrefAttribute) :
                 null;

                var getcontentlength = prop?.Elements().Where(x => x.Name.LocalName == "getcontentlength").FirstOrDefault();
                _ = int.TryParse(getcontentlength?.Value, out var length);

                var creationdate = prop?.Elements().Where(x => x.Name.LocalName == "creationdate").FirstOrDefault();
                _ = DateTime.TryParse(creationdate?.Value, out var created);

                var getlastmodified = prop?.Elements().Where(x => x.Name.LocalName == "getlastmodified").FirstOrDefault();
                _ = DateTime.TryParse(getlastmodified?.Value, out var modified);

                var guardian_restricted = prop?.Elements().Where(x => x.Name.LocalName == "guardian-restricted").FirstOrDefault();
                var guardianRestrictedAttribute =
                    guardian_restricted
                    is not null ?
                    new Attribute(guardian_restricted.Name.LocalName, guardian_restricted.Name.NamespaceName, guardian_restricted.Value) :
                    null;

                var getctag = prop?.Elements().Where(x => x.Name.LocalName == "getctag").FirstOrDefault();
                var getctagAttribute =
                   getctag
                   is not null ?
                   new Attribute(getctag.Name.LocalName, getctag.Name.NamespaceName, getctag.Value) :
                   null;

                var resourcetype = prop?.Elements().Where(x => x.Name.LocalName == "resourcetype").FirstOrDefault();
                var resourcetypeAttributes = resourcetype?.Elements()?.
                    Select(element => new Attribute(element.Name.LocalName, element.Name.NamespaceName, element.Value)).
                    ToList() ??
                    Enumerable.Empty<Attribute>().ToList();

                var getetag = prop?.Elements().Where(x => x.Name.LocalName == "getetag").FirstOrDefault();

                var address_data = prop?.Elements().Where(x => x.Name.LocalName == "address-data").FirstOrDefault();
                var address_dataAttribute =
                    address_data
                    is not null ?
                    new Attribute(address_data.Name.LocalName, address_data.Name.NamespaceName, address_data.Value) :
                    null;

                var item = new Response(
                    href?.Value,
                    status?.Value,
                    length,
                    getetag?.Value,
                    getctagAttribute,
                    me_cardAttribute,
                    guardianRestrictedAttribute,
                    address_dataAttribute,
                    new ReadOnlyCollection<Attribute>(resourcetypeAttributes),
                    created,
                    modified);

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
        public string Href { get; }

        public string Status { get; }

        public int Contentlength { get; }

        public string Etag { get; }

        public Attribute Ctag { get; }

        public Attribute MeCard { get; }

        public Attribute GuardianRestricted { get; }

        public Attribute AddressData { get; }

        public IReadOnlyList<Attribute> ResourceType { get; }

        public DateTime Created { get; }

        public DateTime Modified { get; }

        public Response(
            string href,
            string status,
            int contentlength,
            string etag,
            Attribute ctag,
            Attribute meCard,
            Attribute guardianRestricted,
            Attribute addressData,
            IReadOnlyList<Attribute> resourceType,
            DateTime created,
            DateTime modified)
        {
            Href = href;
            Status = status;
            Contentlength = contentlength;
            Etag = etag;
            Ctag = ctag;
            MeCard = meCard;
            GuardianRestricted = guardianRestricted;
            AddressData = addressData;
            ResourceType = resourceType;
            Created = created;
            Modified = modified;
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
}
