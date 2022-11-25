using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes;

[TypeConverter(typeof(ReminderListConverter))]
[XmlDeserializeType(typeof(MultiStatus))]
public class Reminders
{
    internal Reminders(IEnumerable<Reminder> items) => Items = items;

    public IEnumerable<Reminder> Items { get; set; }
}
