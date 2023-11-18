using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Serialization;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.Dav.Calendar.Extensions;

internal static class VTimeZoneExtensions
{
    public static VTimeZone ToVTimeZone(this string data)
    {
        var byteArray = Encoding.UTF8.GetBytes(data);
        using var stream = new MemoryStream(byteArray);
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        var calendars = CalendarDeserializer.Default.Deserialize(streamReader).OfType<Ical.Net.Calendar>().ToList();
        var vTimeZone = calendars.First().TimeZones.OfType<VTimeZone>().First();
        return vTimeZone;
    }
}