using System.ComponentModel;
using System.Globalization;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Extensions;
using iCloud.Dav.Calendar.WebDav.DataTypes;
using iCloud.Dav.Core.Extensions;

namespace iCloud.Dav.Calendar.Serialization.Converters;

internal sealed class ReminderListConverter : TypeConverter
{
    private const string RemindersKind = "reminders";

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (!CanConvertFrom(context, value.GetType()))
            throw GetConvertFromException(value);

        var multiStatus = (MultiStatus)value;
        var response = multiStatus.Responses.FirstOrDefault(x => x.IsCalendar());
        var propsStat = response?.GetSuccessPropStat();
        var items = multiStatus.Responses.Where(x => x.IsOK())
                                         .Except([response])
                                         .Select(ToReminder)
                                         .ToList();

        return new Reminders()
        {
            Kind = RemindersKind,
            ETag = propsStat?.Prop.GetETag.Value,
            NextSyncToken = propsStat?.Prop.SyncToken?.Value ?? multiStatus.SyncToken?.Value,
            Items = items
        };
    }

    private static Reminder ToReminder(Response response)
    {
        response.ThrowIfNull(nameof(response));
        var propStat = response.GetSuccessPropStat().ThrowIfNull(nameof(PropStat));

        var calendarReminder = propStat.Prop.CalendarData.Value.ToReminder();
        calendarReminder.ETag = propStat.Prop.GetETag.Value;
        calendarReminder.Href = response.Href.Value;
        return calendarReminder;
    }
}