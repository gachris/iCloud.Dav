using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Responses;
using iCloud.Dav.People.Types;
using iCloud.vCard.Net.Serialization.Read;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vCardLib.Deserializers;

namespace iCloud.Dav.People.Utils;

internal static class MappingExtensions
{
    private static readonly ContactReader _contactReader = new();
    private static readonly ContactGroupReader _contactGroupReader = new();

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

    public static ContactGroup ToContactGroup(this string text)
    {
        var contactGroup = new ContactGroup();
        var bytes = Encoding.UTF8.GetBytes(text);
        _contactGroupReader.ReadInto(contactGroup, new StringReader(Encoding.UTF8.GetString(bytes)));
        return contactGroup;
    }

    public static ContactResponse ToContactList(this IEnumerable<Response> responses) => new(responses.Select(ToContact));

    public static Contact ToContact(this Response response)
    {
        var contact = response.AddressData.Value.ToContact();
        contact.ETag = response.Etag;
        return contact;
    }

    public static Contact ToContact(this string text)
    {
        var contact = new Contact();
        text = text.Replace($"{Environment.NewLine} ", string.Empty);

        using var stringReader = new StringReader(text);
        _contactReader.ReadInto(contact, stringReader);

        return contact;
    }
}
