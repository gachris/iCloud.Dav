using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.WebDav.DataTypes;

namespace iCloud.Dav.Calendar.Serialization.Converters;

internal sealed class CalendarListConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

        var multiStatus = (MultiStatus)value;
        var collectionResponse = multiStatus.Responses.FirstOrDefault(x => x.IsCollection());
        var propStat = collectionResponse?.GetSuccessPropStat();
        var items = multiStatus.Responses.Except(new HashSet<Response>() { collectionResponse })
                                         .Select(x => x.ToCalendar())
                                         .ToList();

        return new CalendarList()
        {
            NextSyncToken = propStat?.Prop.SyncToken?.Value ?? multiStatus.SyncToken?.Value,
            ETag = propStat?.Prop.GetETag?.Value,
            Items = items
        };
    }
}