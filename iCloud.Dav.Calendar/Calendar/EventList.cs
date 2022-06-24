using iCloud.Dav.ICalendar.Converters;
using iCloud.Dav.ICalendar.Types;
using iCloud.Dav.Core.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.ICalendar
{
    [TypeConverter(typeof(EventListConverter))]
    [XmlDeserializeType(typeof(Multistatus<Prop>))]
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
