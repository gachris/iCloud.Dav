using System.Linq;
using vCard.Net;
using vCard.Net.Collections;

namespace iCloud.Dav.People.DataTypes
{
    public class CardDataTypePropertyList : GroupedValueList<string, ICardProperty, CardProperty, object>
    {
        public CardDataTypePropertyList() { }

        public ICardProperty this[string name] => ContainsKey(name)
            ? AllOf(name).FirstOrDefault()
            : null;
    }
}