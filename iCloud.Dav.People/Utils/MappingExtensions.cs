using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Responses;
using iCloud.Dav.People.Types;
using iCloud.vCard.Net.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.Dav.People.Utils;

internal static class MappingExtensions
{
    public static IdentityCardResponse ToIdentityCardList(this IEnumerable<Response> responses) =>
        new(responses.Where(response => response.Href.Split('/', StringSplitOptions.RemoveEmptyEntries).Length == 3).Select(ToIdentityCard));

    public static IdentityCard ToIdentityCard(this Response response)
    {
        var resource = response.Href.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
        return new(resource, resource, response.Href.ThrowIfNull(nameof(response.Href)));
    }

    public static ContactGroupResponse ToContactGroupList(this IEnumerable<Response> responses) => new(responses.Select(ToContactGroup));

    public static ContactGroup ToContactGroup(this Response response)
    {
        var contactGroup = response.AddressData.Value.ToContactGroup();
        contactGroup.ETag = response.Etag.ThrowIfNull(nameof(response.Etag));
        return contactGroup;
    }

    public static ContactGroup ToContactGroup(this string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        using var stream = new MemoryStream(bytes);
        return CardDeserializer.Default.Deserialize<ContactGroup>(new StreamReader(stream, Encoding.UTF8));
    }

    public static ContactResponse ToContactList(this IEnumerable<Response> responses) => new(responses.Select(ToContact));

    public static Contact ToContact(this Response response)
    {
        var contact = response.AddressData.Value.ToContact();
        contact.ETag = response.Etag;
        return contact;
    }

    public static Contact ToContact(this string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        using var stream = new MemoryStream(bytes);
        return CardDeserializer.Default.Deserialize<Contact>(new StreamReader(stream, Encoding.UTF8));
    }
}
