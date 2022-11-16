using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.Types;

[TypeConverter(typeof(ReminderConverter))]
public class Reminder : Todo, IDirectResponseSchema
{
    /// <inheritdoc/>
    public virtual string? ETag { get; set; }
}
