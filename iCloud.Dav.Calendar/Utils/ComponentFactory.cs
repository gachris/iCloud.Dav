using Ical.Net;
using Ical.Net.Interfaces.Components;
using Ical.Net.Interfaces.Serialization.Factory;

namespace iCloud.Dav.Calendar.Utils
{
    public class ComponentFactory : ICalendarComponentFactory
    {
        public virtual ICalendarComponent Build(string objectName)
        {
            string upper = objectName.ToUpper();
            ICalendarComponent calendarComponent;
            switch (upper)
            {
                case "VALARM":
                    calendarComponent = new Alarm();
                    break;
                case "VCALENDAR":
                    calendarComponent = new Ical.Net.Calendar();
                    break;
                case "VEVENT":
                    calendarComponent = new Event();
                    break;
                case "VFREEBUSY":
                    calendarComponent = new FreeBusy();
                    break;
                case "VJOURNAL":
                    calendarComponent = new Journal();
                    break;
                case "VTIMEZONE":
                    calendarComponent = new VTimeZone();
                    break;
                case "VTODO":
                    calendarComponent = new Reminder();
                    break;
                default:
                    calendarComponent = new CalendarComponent();
                    break;
            }
            calendarComponent.Name = upper;
            return calendarComponent;
        }
    }
}
