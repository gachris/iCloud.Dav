using iCloud.Dav.Core.Attributes;
using iCloud.Dav.People.Converters;
using iCloud.Dav.People.Types;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People
{
    [TypeConverter(typeof(PersonListConverter))]
    [XmlDeserializeType(typeof(Multistatus<Prop>))]
    public class PersonList : List<Person>, IList<Person>, IEnumerable<Person>, IList, IEnumerable
    {
        public PersonList()
        {
        }

        public PersonList(int capacity) : base(capacity)
        {
        }

        public PersonList(IEnumerable<Person> collection) : base(collection)
        {
        }
    }
}