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
                CalendarEntryList calendarList = new CalendarEntryList();
                foreach (var response in responses)
                {
                    CalendarEntry calendarListEntry = response.ToCalendar();
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
                if (CalendarUrl.Count() == 3)
                {
                    CalendarEntry calendarListEntry = new CalendarEntry();

                    string id = CalendarUrl.Last();
                    string summary = response.Propstat.Prop.Displayname?.Value ?? id;
                    IEnumerable<string> privileges = response.Propstat.Prop.Currentuserprivilegeset?.Privilege?
                        .Select(privilege => (string)privilege.Value) ?? new List<string>();
                    IEnumerable<string> supportedReports = response.Propstat.Prop.Supportedreportset?.Supportedreport?
                        .Select(supportedReport => (string)supportedReport.Report?.Value) ?? new List<string>();
                    IEnumerable<string> supportedCalendarComponents = response.Propstat.Prop.SupportedCalendarComponentSet?.Comps?
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

        public static EventList ToEventList(this List<Response<Prop>> responses)
        {
            List<Event> events = new List<Event>();
            foreach (var multistatusItem in responses)
            {
                if (multistatusItem.Propstat != null
                    && multistatusItem.Propstat.Prop != null
                    && multistatusItem.Propstat.Prop.Calendardata != null)
                {
                    Event @event = multistatusItem.Propstat.Prop.Calendardata.Value.ToEvent();
                    @event.ETag = multistatusItem.Propstat.Prop.Getetag.Value;
                    events.Add(@event);
                }
            }
            return new EventList(events);
        }

        public static Event ToEvent(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(input);
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    var calendars = Event.LoadFromStream(stream);
                    if (calendars != null)
                    {
                        var currentCalendar = calendars.FirstOrDefault();
                        var currentEvent = (Event)currentCalendar.Events[0];
                        return currentEvent;
                    }
                }
            }
            return default;
        }

        public static ReminderList ToReminderList(this List<Response<Prop>> responses)
        {
            List<Reminder> events = new List<Reminder>();
            foreach (var multistatusItem in responses)
            {
                if (multistatusItem.Propstat != null
                    && multistatusItem.Propstat.Prop != null
                    && multistatusItem.Propstat.Prop.Calendardata != null)
                {
                    Reminder @event = multistatusItem.Propstat.Prop.Calendardata.Value.ToReminder();
                    @event.ETag = multistatusItem.Propstat.Prop.Getetag.Value;
                    events.Add(@event);
                }
            }
            return new ReminderList(events);
        }

        public static Reminder ToReminder(this string input)
        {
            if (!String.IsNullOrEmpty(input))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(input);
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    var calendars = Reminder.LoadFromStream(stream);
                    if (calendars != null)
                    {
                        var currentCalendar = calendars.FirstOrDefault();
                        var currentEvent = (Reminder)currentCalendar.Todos[0];
                        return currentEvent;
                    }
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
