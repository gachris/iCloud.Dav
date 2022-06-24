using iCloud.Dav.Calendar.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.Dav.Calendar.Utils
{
    internal static class ConverterHelper
    {
        public static CalendarEntryList ToCalendarList(this List<Response<Prop>> responses)
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

        public static CalendarEntry ToCalendar(this Response<Prop> response)
        {
            if (response != null)
            {
                var CalendarUrl = response.Url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (CalendarUrl.Length == 3)
                {
                    var calendarListEntry = new CalendarEntry();

                    var id = CalendarUrl.Last();
                    var summary = response.Propstat.Prop.Displayname?.Value ?? id;
                    var privileges = response.Propstat.Prop.Currentuserprivilegeset?.Privilege?
                        .Select(privilege => (string)privilege.Value) ?? new List<string>();
                    var supportedReports = response.Propstat.Prop.Supportedreportset?.Supportedreport?
                        .Select(supportedReport => (string)supportedReport.Report?.Value) ?? new List<string>();
                    var supportedCalendarComponents = response.Propstat.Prop.SupportedCalendarComponentSet?.Comps?
                        .Select(comp => comp.Name) ?? new List<string>();

                    calendarListEntry.Id = id;
                    calendarListEntry.Summary = summary;
                    calendarListEntry.Url = response.Url;
                    calendarListEntry.ETag = response.Propstat.Prop.Getetag?.Value;
                    calendarListEntry.CTag = response.Propstat.Prop.Getctag?.Value;
                    calendarListEntry.Privileges.AddRange(privileges);
                    calendarListEntry.SupportedReports.AddRange(supportedReports);
                    calendarListEntry.SupportedCalendarComponents.AddRange(supportedCalendarComponents);
                    calendarListEntry.Color = response.Propstat.Prop.CalendarColor?.Value;
                    return calendarListEntry;
                }
            }
            return default;
        }

        public static EventList ToEventList(this IEnumerable<Response<Prop>> responses)
        {
            var events = new List<Event>();
            foreach (var multistatusItem in responses)
            {
                if (multistatusItem.Propstat != null
                    && multistatusItem.Propstat.Prop != null
                    && multistatusItem.Propstat.Prop.Calendardata != null)
                {
                    var @event = multistatusItem.Propstat.Prop.Calendardata.Value.ToEvent();
                    @event.ETag = multistatusItem.Propstat.Prop.Getetag.Value;
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

        public static ReminderList ToReminderList(this List<Response<Prop>> responses)
        {
            var events = new List<Reminder>();
            foreach (var multistatusItem in responses)
            {
                if (multistatusItem.Propstat != null
                    && multistatusItem.Propstat.Prop != null
                    && multistatusItem.Propstat.Prop.Calendardata != null)
                {
                    var @event = multistatusItem.Propstat.Prop.Calendardata.Value.ToReminder();
                    @event.ETag = multistatusItem.Propstat.Prop.Getetag.Value;
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
}
