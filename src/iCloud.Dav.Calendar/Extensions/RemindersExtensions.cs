using System.Text;
using Ical.Net.Serialization;
using iCloud.Dav.Calendar.DataTypes;
using iCloud.Dav.Calendar.Serialization;

namespace iCloud.Dav.Calendar.Extensions;

internal static class RemindersExtensions
{
    public static Reminder ToReminder(this string data)
    {
        var byteArray = Encoding.UTF8.GetBytes(data);
        using var stream = new MemoryStream(byteArray);
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        var calendars = CalendarDeserializer.Default.Deserialize(streamReader).OfType<Ical.Net.Calendar>().ToList();
        var calendarReminder = calendars.First().Todos.OfType<Reminder>().First();
        return calendarReminder;
    }

    public static string SerializeToString(this Reminder reminder)
    {
        var serializer = new CalendarSerializer();
        return serializer.SerializeToString(reminder.Calendar);
    }
}