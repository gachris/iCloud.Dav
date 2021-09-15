using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace iCloud.Dav.People
{
    /// <summary>
    ///  Provides the vCardAddressCollection.
    /// </summary>
    public class vCardAddressCollection : Collection<VCardAddress>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="vCardAddressCollection"/>
        /// class that is empty.
        /// </summary>
        public vCardAddressCollection() : base()
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="vCardAddressCollection"/>
        ///  class as a wrapper for the specified list.
        /// </summary>
        /// <param name="list">The list that is wrapped by the new collection.</param>
        /// <exception cref="System.ArgumentNullException">list is null.</exception>
        public vCardAddressCollection(IList<VCardAddress> list) : base(list)
        {
        }
    }
}