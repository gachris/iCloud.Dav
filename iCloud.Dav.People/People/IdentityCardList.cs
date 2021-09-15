using iCloud.Dav.Core.Attributes;
using iCloud.Dav.People.Converters;
using iCloud.Dav.People.Types;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People
{
    [TypeConverter(typeof(IdentityCardListConverter))]
    [XmlDeserializeType(typeof(Multistatus<Prop>))]
    public class IdentityCardList : List<IdentityCard>, IList<IdentityCard>, IEnumerable<IdentityCard>, IList, IEnumerable
    {
        public IdentityCardList()
        {
        }

        public IdentityCardList(int capacity) : base(capacity)
        {
        }

        public IdentityCardList(IEnumerable<IdentityCard> collection) : base(collection)
        {
        }
    }
}
