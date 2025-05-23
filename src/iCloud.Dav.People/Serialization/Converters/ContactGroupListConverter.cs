﻿using System.ComponentModel;
using System.Globalization;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.WebDav.DataTypes;

namespace iCloud.Dav.People.Serialization.Converters;

internal sealed class ContactGroupListConverter : TypeConverter
{
    private const string GroupsKind = "groups";

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (!CanConvertFrom(context, value.GetType()))
            throw GetConvertFromException(value);

        var multiStatus = (MultiStatus)value;
        var response = multiStatus.Responses.FirstOrDefault(x => x.IsAddressbook());
        var items = multiStatus.Responses.Where(x => x.IsOK() && x.IsGroup())
                                         .Except(new HashSet<Response>() { response })
                                         .Select(ToContactGroup)
                                         .ToList();

        return new ContactGroupList()
        {
            Kind = GroupsKind,
            Items = items
        };
    }

    private static ContactGroup ToContactGroup(Response response)
    {
        if (response is null)
            throw new ArgumentNullException(nameof(response));
        if (response.GetSuccessPropStat() is not PropStat propStat)
            throw new ArgumentNullException(nameof(propStat));

        var contactGroup = propStat.Prop.AddressData.Value.ToContactGroup();
        contactGroup.ETag = propStat.Prop.GetETag.Value;
        contactGroup.Href = response.Href.Value;
        return contactGroup;
    }
}