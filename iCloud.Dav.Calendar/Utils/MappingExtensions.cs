using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Serialization;
using iCloud.Dav.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.Dav.Calendar.Utils;

internal static class MappingExtensions
{
    public static CalendarList ToCalendarList(this IEnumerable<Response> responses)
    {
        if (responses is null) throw new ArgumentNullException(nameof(responses));
        return new(responses.Select(ToCalendar));
    }

    public static DataTypes.Calendar ToCalendar(this Response response)
    {
        if (response is null) throw new ArgumentNullException(nameof(response));
        var id = response.Href.Split('/', StringSplitOptions.RemoveEmptyEntries).Last();
        var calendarListEntry = new DataTypes.Calendar()
        {
            Uid = id,
            ETag = response.Etag,
            CTag = response.Ctag?.Value,
            Color = response.CalendarColor,
            Summary = response.DisplayName ?? id,
            SyncToken = response.SyncToken
        };
        calendarListEntry.Privileges.AddRange(response.CurrentUserPrivilegeSet.Select(privilege => privilege.Name));
        calendarListEntry.SupportedReports.AddRange(response.SupportedReportSet.Select(supportedReport => supportedReport.Name));
        calendarListEntry.SupportedCalendarComponents.AddRange(response.SupportedCalendarComponentSet.Select(comp => comp.Name));
        return calendarListEntry;
    }

    public static Events ToEventList(this IEnumerable<Response> responses)
    {
        if (responses is null) throw new ArgumentNullException(nameof(responses));
        return new Events(responses.Where(x => x.CalendarData is not null).Select(ToEvent));
    }

    public static Event ToEvent(this Response response)
    {
        var calendarDataAttribute = response.CalendarData.ThrowIfNull(nameof(response.CalendarData));
        var calendarData = calendarDataAttribute.Value.ThrowIfNull(nameof(response.CalendarData.Value));
        var calendarEvent = calendarData.ToEvent();
        calendarEvent.ETag = response.Etag;
        return calendarEvent;
    }

    public static Event ToEvent(this string data)
    {
        var byteArray = Encoding.UTF8.GetBytes(data);
        using var stream = new MemoryStream(byteArray);
        var calendars = CalendarDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).OfType<Ical.Net.Calendar>().ToList();
        var calendarEvent = calendars.First().Events.OfType<Event>().First();
        return calendarEvent;
    }

    public static Reminders ToReminderList(this IEnumerable<Response> responses)
    {
        if (responses is null) throw new ArgumentNullException(nameof(responses));
        return new Reminders(responses.Where(x => x.CalendarData is not null).Select(ToReminder));
    }

    public static Reminder ToReminder(this Response response)
    {
        var calendarDataAttribute = response.CalendarData.ThrowIfNull(nameof(response.CalendarData));
        var calendarData = calendarDataAttribute.Value.ThrowIfNull(nameof(response.CalendarData.Value));
        var calendarReminder = calendarData.ToReminder();
        calendarReminder.ETag = response.Etag;
        return calendarReminder;
    }

    public static Reminder ToReminder(this string data)
    {
        var byteArray = Encoding.UTF8.GetBytes(data);
        using var stream = new MemoryStream(byteArray);
        var calendars = CalendarDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).OfType<Ical.Net.Calendar>().ToList();
        var calendarReminder = calendars.First().Todos.OfType<Reminder>().First();
        return calendarReminder;
    }

    public static string? ToFilterTime(this DateTime? dateTime)
    {
        if (dateTime is null) return default;
        var universalTime = dateTime.Value.ToUniversalTime();
        var universalTimeString = universalTime.ToString("s");
        universalTimeString = universalTimeString.Replace("-", "");
        universalTimeString = universalTimeString.Replace(":", "");
        universalTimeString = string.Concat(universalTimeString, "Z");
        return universalTimeString;
    }
}
