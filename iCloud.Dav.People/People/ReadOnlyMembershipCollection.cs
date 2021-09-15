using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace iCloud.Dav.People
{
    public class ReadOnlyMembershipCollection : ReadOnlyCollection<Membership>
    {
        public ReadOnlyMembershipCollection(IList<Membership> list) : base(list)
        {
        }
    }
}