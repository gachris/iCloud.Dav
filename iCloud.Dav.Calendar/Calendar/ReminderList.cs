using iCloud.Dav.ICalendar.Converters;
using iCloud.Dav.ICalendar.Types;
using iCloud.Dav.Core.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.ICalendar
{
    [TypeConverter(typeof(ReminderListConverter))]
    [XmlDeserializeType(typeof(Multistatus<Prop>))]
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
