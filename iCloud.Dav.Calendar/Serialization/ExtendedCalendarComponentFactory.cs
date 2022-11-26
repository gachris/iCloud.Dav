using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;
using iCloud.Dav.Calendar.DataTypes;

namespace iCloud.Dav.Calendar.Serialization
{
    internal class ExtendedCalendarComponentFactory : CalendarComponentFactory
    {
        public override ICalendarComponent Build(string objectName)
        {
            var text = objectName.ToUpper();
            var calendarComponent = default(ICalendarComponent);
            switch (text)
            {
                case "VEVENT":
                    calendarComponent = new Event();
                    break;
                case "VTODO":
                    calendarComponent = new Reminder();
                    break;
                default:
                    break;
            }

            if (calendarComponent is null)
            {
                return base.Build(objectName);
            }
            else
            {
                calendarComponent.Name = text;
                return calendarComponent;
            }
        }
    }
}