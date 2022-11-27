﻿using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;
using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.Dav.Calendar.Utils
{
    internal static class MappingExtensions
    {
        public static CalendarListEntry ToCalendar(this Response response)
        {
            if (response is null) throw new ArgumentNullException(nameof(response));
            var id = response.Href.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
            var calendarListEntry = new CalendarListEntry()
            {
                Href = response.Href,
                Uid = id,
                ETag = response.Etag,
                CTag = response.Ctag?.Value,
                Color = response.CalendarColor,
                Summary = response.DisplayName,
                Description = response.CalendarDescription,
                Order = response.CalendarOrder,
            };

            if (response.Status == Status.NotFound)
            {
                calendarListEntry.Deleted = true;
                return calendarListEntry;
            }

            if (!string.IsNullOrEmpty(response.CalendarTimeZone))
            {
                var byteArray = Encoding.UTF8.GetBytes(response.CalendarTimeZone);
                using (var stream = new MemoryStream(byteArray))
                {
                    var calendars = CalendarDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).OfType<Ical.Net.Calendar>().ToList();
                    calendarListEntry.TimeZone = calendars.First().TimeZones.OfType<VTimeZone>().First();
                }
            }
            calendarListEntry.Privileges.AddRange(response.CurrentUserPrivilegeSet.Select(privilege => privilege.Name));
            calendarListEntry.SupportedReports.AddRange(response.SupportedReportSet.Select(supportedReport => supportedReport.Name));
            calendarListEntry.SupportedCalendarComponents.AddRange(response.SupportedCalendarComponentSet.Select(comp => comp.Name));

            return calendarListEntry;
        }

        public static Event ToEvent(this string data)
        {
            var byteArray = Encoding.UTF8.GetBytes(data);
            using (var stream = new MemoryStream(byteArray))
            {
                var calendars = CalendarDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).OfType<Ical.Net.Calendar>().ToList();
                var calendarEvent = calendars.First().Events.OfType<Event>().First();
                return calendarEvent;
            }
        }

        public static Reminder ToReminder(this string data)
        {
            var byteArray = Encoding.UTF8.GetBytes(data);
            using (var stream = new MemoryStream(byteArray))
            {
                var calendars = CalendarDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).OfType<Ical.Net.Calendar>().ToList();
                var calendarReminder = calendars.First().Todos.OfType<Reminder>().First();
                return calendarReminder;
            }
        }

        public static string SerializeToString(this Event calendarEvent)
        {
            var calendar = new Ical.Net.Calendar();
            var calendarSerializer = new CalendarSerializer();
            calendar.Events.Add(calendarEvent);
            return calendarSerializer.SerializeToString(calendar);
        }

        public static string SerializeToString(this Reminder calendarReminder)
        {
            var calendar = new Ical.Net.Calendar();
            var calendarSerializer = new CalendarSerializer();
            calendar.Todos.Add(calendarReminder);
            return calendarSerializer.SerializeToString(calendar);
        }

        public static string ToFilterTime(this DateTime? dateTime)
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
}