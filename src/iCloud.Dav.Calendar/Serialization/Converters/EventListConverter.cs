using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.WebDav.DataTypes;
using iCloud.Dav.Core.Extensions;

namespace iCloud.Dav.Calendar.Serialization.Converters;

internal sealed class EventListConverter : TypeConverter
{
    private const string EventsKind = "events";

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (!CanConvertFrom(context, value.GetType()))
            throw GetConvertFromException(value);

        var multiStatus = (MultiStatus)value;
        var response = multiStatus.Responses.FirstOrDefault(x => x.IsCalendar());
        var items = multiStatus.Responses.Except(new HashSet<Response>() { response })
                                         .Select(ToEvent)
                                         .ToList();
        var propsStat = response?.GetSuccessPropStat();

        return new Events()
        {
            Kind = EventsKind,
            ETag = propsStat?.Prop.GetETag.Value,
            NextSyncToken = propsStat?.Prop.SyncToken?.Value ?? multiStatus.SyncToken?.Value,
            Items = items
        };
    }

    private static Event ToEvent(Response response)
    {
        response.ThrowIfNull(nameof(response));
        var propStat = response.GetSuccessPropStat().ThrowIfNull(nameof(PropStat));

        var calendarEvent = propStat.Prop.CalendarData.Value.ToEvent();
        calendarEvent.ETag = propStat.Prop.GetETag.Value;
        calendarEvent.Id = response.Href.ExtractId();
        return calendarEvent;
    }
}