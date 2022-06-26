using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace iCloud.Dav.Auth.CardDav.Types
{
    [XmlRoot(ElementName = "multistatus", Namespace = "DAV:")]
    internal sealed class Multistatus : IXmlSerializable
    {
        public List<Response> Responses { get; }

        public Multistatus()
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

                var current_user_principal = prop?.Elements().Where(x => x.Name.LocalName == "current-user-principal").FirstOrDefault();
                var current_user_principal_href = current_user_principal?.Elements().Where(x => x.Name.LocalName == "href").FirstOrDefault();

                var displayname = prop?.Elements().Where(x => x.Name.LocalName == "displayname").FirstOrDefault();

                var calendar_home_set = prop?.Elements().Where(x => x.Name.LocalName == "calendar-home-set").FirstOrDefault();
                var calendar_home_set_href = calendar_home_set?.Elements().Where(x => x.Name.LocalName == "href").FirstOrDefault();

                var addressbook_home_set = prop?.Elements().Where(x => x.Name.LocalName == "addressbook-home-set").FirstOrDefault();
                var addressbook_home_set_href = addressbook_home_set?.Elements().Where(x => x.Name.LocalName == "href").FirstOrDefault();

                var calendar_user_address_set = prop?.Elements().Where(x => x.Name.LocalName == "calendar-user-address-set").FirstOrDefault();
                var calendar_user_address_set_href = calendar_user_address_set?.Elements().Where(x => x.Name.LocalName == "href");
                var calendar_user_address = calendar_user_address_set_href?.
                    Select(element => new CalendarUserAddress(element.Attributes().Where(x => x.Name == "preferred").FirstOrDefault()?.Value, element.Value)).
                    ToList() ??
                    Enumerable.Empty<CalendarUserAddress>().ToList();

                var item = new Response(
                    href?.Value?.Trim(),
                    current_user_principal?.Value?.Trim(),
                    calendar_home_set_href?.Value?.Trim(),
                    addressbook_home_set_href?.Value?.Trim(),
                    displayname?.Value?.Trim(),
                    calendar_user_address);

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

        public string CurrentUserPrincipal { get; }

        public string CalendarHomeSet { get; }

        public string AddressBookHomeSet { get; }

        public string DisplayName { get; }

        public IReadOnlyCollection<CalendarUserAddress> CalendarUserAddressSet { get; }

        public Response(string href, string currentUserPrincipal, string calendarHomeSet, string addressBookHomeSet, string displayName, List<CalendarUserAddress> calendarUserAddressSet)
        {
            Href = href;
            CurrentUserPrincipal = currentUserPrincipal;
            CalendarHomeSet = calendarHomeSet;
            AddressBookHomeSet = addressBookHomeSet;
            DisplayName = displayName;
            CalendarUserAddressSet = calendarUserAddressSet;
        }
    }

    internal sealed class CalendarUserAddress
    {
        public string Preferred { get; }

        public string Value { get; }

        public CalendarUserAddress(string preferred, string value)
        {
            Preferred = preferred;
            Value = value;
        }
    }
}
