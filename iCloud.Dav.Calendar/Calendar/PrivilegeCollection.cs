using System.Collections;
using System.Collections.Generic;

namespace iCloud.Dav.Calendar
{
    public class PrivilegeCollection : List<string>, IEnumerable, IEnumerable<string>, ICollection, ICollection<string>, IList, IList<string>
    {
        public PrivilegeCollection()
        {
        }

        public PrivilegeCollection(int capacity) : base(capacity)
        {
        }

        public PrivilegeCollection(IEnumerable<string> collection) : base(collection)
        {
        }
    }
}
