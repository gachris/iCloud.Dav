using iCloud.Dav.Calendar.Cal.Types;
using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Core.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar
{
    [TypeConverter(typeof(EventListConverter))]
    [XmlDeserializeType(typeof(MultiStatus))]
    public class EventList : List<Event>, IEnumerable, IEnumerable<Event>, ICollection, ICollection<Event>, IList, IList<Event>
    {
        public EventList()
        {
        }

        public EventList(int capacity) : base(capacity)
        {
        }

        public EventList(IEnumerable<Event> collection) : base(collection)
        {
        }
    }
}
