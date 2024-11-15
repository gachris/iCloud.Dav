using System.Text;
using Ical.Net.Serialization;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Serialization;

namespace iCloud.Dav.Calendar.Extensions;

internal static class EventExtensions
{
    public static Event ToEvent(this string data)
    {
        var byteArray = Encoding.UTF8.GetBytes(data);
        using var stream = new MemoryStream(byteArray);
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        var calendars = CalendarDeserializer.Default.Deserialize(streamReader).OfType<Ical.Net.Calendar>().ToList();
        var calendarEvent = calendars.First().Events.OfType<Event>().First();
        return calendarEvent;
    }

    public static string SerializeToString(this Event calendarEvent)
    {
        var serializer = new CalendarSerializer();
        return serializer.SerializeToString(calendarEvent.Calendar);
    }
}