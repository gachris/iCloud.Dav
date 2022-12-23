using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Serialization.Converters;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Represents a strongly typed list of objects that can be accessed by index. Provides
    /// methods to search, sort, and manipulate lists.
    /// </summary>   
    [TypeConverter(typeof(ContactGroupListConverter))]
    [XmlDeserializeType(typeof(MultiStatus))]
    public class ContactGroupList
    {
        /// <summary>
        /// The list of groups that the requestor is connected to.
        /// </summary>
        public virtual IList<ContactGroup> Items { get; set; }

        /// <summary>
        /// Type of the collection ("people#groups").
        /// </summary>
        public virtual string Kind { get; set; }
    }
}