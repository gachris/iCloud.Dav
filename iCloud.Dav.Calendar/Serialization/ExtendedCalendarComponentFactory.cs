using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;
using iCloud.Dav.Calendar.DataTypes;

namespace iCloud.Dav.Calendar.Serialization;

internal class ExtendedCalendarComponentFactory : CalendarComponentFactory
{
    public override ICalendarComponent Build(string objectName)
    {
        var text = objectName.ToUpper();
        var calendarComponent = text switch
        {
            "VEVENT" => new Event(),
            "VTODO" => new Reminder(),
            _ => (ICalendarComponent?)null,
        };

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
