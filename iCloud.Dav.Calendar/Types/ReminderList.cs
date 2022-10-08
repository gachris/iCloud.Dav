using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Core;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.Types;

[TypeConverter(typeof(ReminderListConverter))]
[XmlDeserializeType(typeof(MultiStatus))]
public class ReminderList : List<Reminder>, IEnumerable, IEnumerable<Reminder>, ICollection, ICollection<Reminder>, IList, IList<Reminder>
{
    public ReminderList()
    {
    }

    public ReminderList(int capacity) : base(capacity)
    {
    }

    public ReminderList(IEnumerable<Reminder> collection) : base(collection)
    {
    }
}
