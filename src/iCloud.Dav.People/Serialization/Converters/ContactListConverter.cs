using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.WebDav.DataTypes;

namespace iCloud.Dav.People.Serialization.Converters;

internal sealed class ContactListConverter : TypeConverter
{
    private const string ContactsKind = "contacts";

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (!CanConvertFrom(context, value.GetType()))
            throw GetConvertFromException(value);

        var multiStatus = (MultiStatus)value;
        var response = multiStatus.Responses.FirstOrDefault(x => x.IsAddressbook());
        var items = multiStatus.Responses.Where(x => x.IsOK() && !x.IsGroup())
                                         .Except(new HashSet<Response>() { response })
                                         .Select(ToContact)
                                         .ToList();

        return new ContactList()
        {
            Kind = ContactsKind,
            Items = items
        };
    }

    private static Contact ToContact(Response response)
    {
        response.ThrowIfNull(nameof(response));
        var propStat = response.GetSuccessPropStat().ThrowIfNull(nameof(PropStat));

        var contact = propStat.Prop.AddressData.Value.ToContact();
        contact.ETag = propStat.Prop.GetETag.Value;
        contact.Id = response.Href.ExtractId();
        return contact;
    }
}