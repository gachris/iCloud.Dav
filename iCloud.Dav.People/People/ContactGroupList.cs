using iCloud.Dav.Core.Attributes;
using iCloud.Dav.People.Converters;
using iCloud.Dav.People.Types;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People
{
    /// <summary>
    /// Represents a strongly typed list of objects that can be accessed by index. Provides
    /// methods to search, sort, and manipulate lists.
    /// </summary>   
    [TypeConverter(typeof(ContactGroupsListConverter))]
    [XmlDeserializeType(typeof(Multistatus<Prop>))]
    public class ContactGroupsList : List<ContactGroup>, IList<ContactGroup>, IEnumerable<ContactGroup>, IList, IEnumerable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactGroupsList"/> class that
        /// is empty and has the default initial capacity.
        /// </summary>
        public ContactGroupsList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactGroupsList"/> class that
        /// is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <exception cref="System.ArgumentNullException">capacity is less than 0.</exception>
        public ContactGroupsList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactGroupsList"/> class that
        /// contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="System.ArgumentNullException"/>
        public ContactGroupsList(IEnumerable<ContactGroup> collection) : base(collection)
        {
        }
    }
}
