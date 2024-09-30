using System.ComponentModel;
using System.Globalization;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.WebDav.DataTypes;

namespace iCloud.Dav.Calendar.Serialization.Converters;

internal sealed class SyncTokenConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

        var multiStatus = (MultiStatus)value;
        var response = multiStatus.Responses.FirstOrDefault(x => x.IsCollection() || x.IsCalendar());

        return response is null
            ? throw new ArgumentNullException(nameof(response))
            : response.GetSuccessPropStat() is not PropStat propStat
            ? throw new ArgumentNullException(nameof(propStat))
            : (object)new DataTypes.SyncToken()
            {
                ETag = propStat.Prop.GetETag?.Value,
                NextSyncToken = propStat.Prop.SyncToken.Value
            };
    }
}