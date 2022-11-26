using iCloud.Dav.Core;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.PeopleComponents;
using iCloud.Dav.People.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vCard.Net.CardComponents;

namespace iCloud.Dav.People.Utils
{
    internal static class MappingExtensions
    {
        public static IdentityCardResponse ToIdentityCardList(this IEnumerable<Response> responses) =>
            new IdentityCardResponse(responses.Where(response => response.Href.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Length == 3).Select(ToIdentityCard));

        public static IdentityCard ToIdentityCard(this Response response)
        {
            var resource = response.Href.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
            return new IdentityCard(resource, resource, response.Href.ThrowIfNull(nameof(response.Href)));
        }

        public static T Deserialize<T>(this Response response) where T : ICardComponent, IDirectResponseSchema
        {
            var contact = response.AddressData.Value.Deserialize<T>();
            contact.ETag = response.Etag;
            return contact;
        }

        public static T Deserialize<T>(this string data) where T : ICardComponent
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            using (var stream = new MemoryStream(bytes))
                return (T)CardDeserializer.Default.Deserialize<T>(new StreamReader(stream, Encoding.UTF8)).First();
        }

        public static string SerializeToString(this ContactGroup data)
        {
            var serializer = new ContactGroupSerializer();
            return serializer.SerializeToString(data);
        }

        public static string SerializeToString(this Contact data)
        {
            var serializer = new ContactSerializer();
            return serializer.SerializeToString(data);
        }
    }
}