using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.WebDav.DataTypes;

namespace iCloud.Dav.People.Serialization.Converters;

internal sealed class IdentityCardListConverter : TypeConverter
{
    private const string ResourcesKind = "resources";

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (!CanConvertFrom(context, value.GetType()))
            throw GetConvertFromException(value);

        var multiStatus = (MultiStatus)value;
        var response = multiStatus.Responses.FirstOrDefault(x => x.IsCollection());
        var propsStat = response?.GetSuccessPropStat();
        var items = multiStatus.Responses.Except(new HashSet<Response>() { response }).Select(ToIdentityCard).ToList();

        var identityCardList = new IdentityCardList()
        {
            Kind = ResourcesKind,
            Items = items,
        };

        if (propsStat != null)
        {
            identityCardList.ETag = propsStat.Prop.GetETag.Value;
            identityCardList.MeCard = propsStat.Prop.MeCard?.Href.ExtractId();
            identityCardList.NextSyncToken = propsStat.Prop.SyncToken?.Value ?? multiStatus.SyncToken?.Value;
        }
        else
        {
            identityCardList.NextSyncToken = multiStatus.SyncToken?.Value;
        }

        if (!identityCardList.Items.Any())
        {
            identityCardList.Items.Add(new IdentityCard()
            {
                ETag = propsStat?.Prop.GetETag.Value,
                NextSyncToken = propsStat?.Prop.SyncToken?.Value ?? multiStatus.SyncToken?.Value,
            });
        }

        return identityCardList;
    }

    private static IdentityCard ToIdentityCard(Response response)
    {
        return response is null
            ? throw new ArgumentNullException(nameof(response))
            : !(response.GetSuccessPropStat() is PropStat propStat)
            ? throw new ArgumentNullException(nameof(propStat))
            : new IdentityCard()
            {
                ResourceName = response.Href.ExtractId(),
                ETag = propStat.Prop.GetETag.Value,
                NextSyncToken = propStat.Prop.SyncToken?.Value
            };
    }
}