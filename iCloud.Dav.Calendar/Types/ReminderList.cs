using iCloud.Dav.Calendar.Cal.Types;
using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Core.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar
{
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
}
