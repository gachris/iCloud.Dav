using System.ComponentModel;
using System.Globalization;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.WebDav.DataTypes;

namespace iCloud.Dav.Calendar.Serialization.Converters;

internal sealed class SyncCollectionListConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

        var multiStatus = (MultiStatus)value;
        var response = multiStatus.Responses.FirstOrDefault(x => x.IsCollection() || x.IsCalendar());
        var propStat = response?.GetSuccessPropStat();
        var items = multiStatus.Responses.Except(new HashSet<Response>() { response })
                                         .Select(ToSyncCollectionItem)
                                         .ToList();

        return new SyncCollectionList()
        {
            NextSyncToken = propStat?.Prop.SyncToken?.Value ?? multiStatus.SyncToken?.Value,
            ETag = propStat?.Prop.GetETag?.Value,
            Items = items
        };
    }

    private static SyncCollectionItem ToSyncCollectionItem(Response response)
    {
        if (response is null)
            throw new ArgumentNullException(nameof(response));

        var propStat = response.GetSuccessPropStat();

        return new SyncCollectionItem()
        {
            Id = response.Href.ExtractId(),
            ETag = propStat?.Prop.GetETag.Value,
            Deleted = response.StatusCode == System.Net.HttpStatusCode.NotFound ? true : (bool?)null
        };
    }
}