using System.Collections;
using System.Collections.Generic;

namespace iCloud.Dav.Auth
{
    /// <summary>
    /// Represents a strongly typed list of objects that can be accessed by index. Provides
    /// methods to search, sort, and manipulate lists.
    /// </summary>
    public class CalendarUserAddressSetList : List<CalendarUserAddressSet>, IEnumerable, IEnumerable<CalendarUserAddressSet>, ICollection, ICollection<CalendarUserAddressSet>, IList, IList<CalendarUserAddressSet>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarUserAddressSetList"/> class that
        /// is empty and has the default initial capacity.
        /// </summary>
        public CalendarUserAddressSetList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarUserAddressSetList"/> class that
        /// is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <exception cref="T:System.ArgumentNullException">capacity is less than 0.</exception>
        public CalendarUserAddressSetList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarUserAddressSetList"/> class that
        /// contains elements copied from the specified collection and has sufficient capacity
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection"></param>
        /// <exception cref="T:System.ArgumentNullException">collection is null.</exception>
        public CalendarUserAddressSetList(IEnumerable<CalendarUserAddressSet> collection) : base(collection)
        {
        }
    }
}
