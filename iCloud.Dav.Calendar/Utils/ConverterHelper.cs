using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.Dav.Calendar.Utils;

internal static class ConverterHelper
{
    public static CalendarEntryList ToCalendarList(this List<Response> responses)
    {
        if (responses != null && responses.Any())
        {
            var calendarList = new CalendarEntryList();
            foreach (var response in responses)
            {
                var calendarListEntry = response.ToCalendar();
                if (calendarListEntry != null)
                {
                    calendarList.Add(calendarListEntry);
                }
            }
            return calendarList;
        }
        return default;
    }

    public static CalendarEntry ToCalendar(this Response response)
    {
        if (response is null) return null;

        var CalendarUrl = response.Href.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (CalendarUrl.Length != 3) return null;

        var id = CalendarUrl.Last();
        var calendarListEntry = new CalendarEntry
        {
            Id = id,
            Url = response.Href,
            ETag = response.Etag,
            CTag = response.Ctag?.Value,
            Color = response.CalendarColor,
            Summary = response.DisplayName ?? id
        };

        var privileges = response.CurrentUserPrivilegeSet.Select(privilege => privilege.Name);
        var supportedReports = response.SupportedReportSet.Select(supportedReport => supportedReport.Name);
        var supportedCalendarComponents = response.SupportedCalendarComponentSet.Select(comp => comp.Name);

        calendarListEntry.Privileges.AddRange(privileges);
        calendarListEntry.SupportedReports.AddRange(supportedReports);
        calendarListEntry.SupportedCalendarComponents.AddRange(supportedCalendarComponents);

        return calendarListEntry;
    }

    public static EventList ToEventList(this IEnumerable<Response> responses)
    {
        var events = new List<Event>();
        foreach (var multistatusItem in responses)
        {
            if (multistatusItem.CalendarData != null)
            {
                var @event = multistatusItem.CalendarData.Value.ToEvent();
                @event.ETag = multistatusItem.Etag;
                events.Add(@event);
            }
        }
        return new EventList(events);
    }

    public static Event ToEvent(this string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            var byteArray = Encoding.UTF8.GetBytes(input);
            using var stream = new MemoryStream(byteArray);
            var calendars = Event.LoadFromStream(stream);
            if (calendars != null)
            {
                var currentCalendar = calendars.FirstOrDefault().Events.FirstOrDefault();
                var currentEvent = currentCalendar;
                return currentEvent as Event;
            }
        }
        return default;
    }

    public static ReminderList ToReminderList(this List<Response> responses)
    {
        var events = new List<Reminder>();
        foreach (var multistatusItem in responses)
        {
            if (multistatusItem.CalendarData != null)
            {
                var @event = multistatusItem.CalendarData.Value.ToReminder();
                @event.ETag = multistatusItem.Etag;
                events.Add(@event);
            }
        }
        return new ReminderList(events);
    }

    public static Reminder ToReminder(this string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            var byteArray = Encoding.UTF8.GetBytes(input);
            using var stream = new MemoryStream(byteArray);
            var calendars = Reminder.LoadFromStream(stream);
            if (calendars != null)
            {
                var currentCalendar = calendars.FirstOrDefault().Todos.FirstOrDefault();
                var currentEvent = currentCalendar;
                return currentEvent as Reminder;
            }
        }
        return default;
    }

    public static string ToFilterTime(this DateTime? dateTime)
    {
        if (dateTime != null)
        {
            var universalTime = dateTime.Value.ToUniversalTime();
            var universalTimeString = universalTime.ToString("s");
            universalTimeString = universalTimeString.Replace("-", "");
            universalTimeString = universalTimeString.Replace(":", "");
            universalTimeString = string.Concat(universalTimeString, "Z");
            return universalTimeString;
        }
        return default;
    }
}
