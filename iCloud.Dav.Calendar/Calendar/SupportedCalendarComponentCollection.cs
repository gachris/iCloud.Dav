using System.Collections;
using System.Collections.Generic;

namespace iCloud.Dav.Calendar
{
    public class SupportedCalendarComponentCollection : List<string>, IEnumerable, IEnumerable<string>, ICollection, ICollection<string>, IList, IList<string>
    {
        public SupportedCalendarComponentCollection()
        {
        }

        public SupportedCalendarComponentCollection(int capacity) : base(capacity)
        {
        }

        public SupportedCalendarComponentCollection(IEnumerable<string> collection) : base(collection)
        {
        }
    }
}