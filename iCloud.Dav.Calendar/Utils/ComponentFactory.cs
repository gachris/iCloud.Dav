﻿using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;

namespace iCloud.Dav.ICalendar.Utils
{
    public class ComponentFactory : CalendarComponentFactory
    {
        public override ICalendarComponent Build(string objectName)
        {
            var text = objectName.ToUpper();
            var calendarComponent = text switch
            {
                "VALARM" => new Alarm(),
                "VEVENT" => new Event(),
                "VFREEBUSY" => new FreeBusy(),
                "VJOURNAL" => new Journal(),
                "VTIMEZONE" => new VTimeZone(),
                "VTODO" => new Reminder(),
                "VCALENDAR" => new Calendar(),
                "DAYLIGHT" or "STANDARD" => new VTimeZoneInfo(),
                _ => new CalendarComponent(),
            };
            calendarComponent.Name = text;
            return calendarComponent;
        }
    }
}
