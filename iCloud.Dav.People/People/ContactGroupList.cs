using iCloud.Dav.Core.Attributes;
using iCloud.Dav.People.Converters;
using iCloud.Dav.People.Types;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People
{
    [TypeConverter(typeof(ContactGroupsListConverter))]
    [XmlDeserializeType(typeof(Multistatus<Prop>))]
    public class ContactGroupsList : List<ContactGroup>, IList<ContactGroup>, IEnumerable<ContactGroup>, IList, IEnumerable
    {
        public ContactGroupsList()
        {
        }

        public ContactGroupsList(int capacity) : base(capacity)
        {
        }

        public ContactGroupsList(IEnumerable<ContactGroup> collection) : base(collection)
        {
        }
    }
}
