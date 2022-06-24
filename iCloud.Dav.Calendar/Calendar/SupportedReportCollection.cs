using System.Collections;
using System.Collections.Generic;

namespace iCloud.Dav.Calendar
{
    public class SupportedReportCollection : List<string>, IEnumerable, IEnumerable<string>, ICollection, ICollection<string>, IList, IList<string>
    {
        public SupportedReportCollection()
        {
        }

        public SupportedReportCollection(int capacity) : base(capacity)
        {
        }

        public SupportedReportCollection(IEnumerable<string> collection) : base(collection)
        {
        }
    }
}