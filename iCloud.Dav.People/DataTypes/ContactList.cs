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
    [TypeConverter(typeof(ContactListConverter))]
    [XmlDeserializeType(typeof(MultiStatus))]
    public class ContactList
    {
        /// <summary>
        /// The list of people that the requestor is connected to.
        /// </summary>
        public virtual IList<Contact> Items { get; set; }

        /// <summary>
        /// Type of the collection ("people#contacts").
        /// </summary>
        public virtual string Kind { get; set; }
    }
}