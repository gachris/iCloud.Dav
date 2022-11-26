using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.PeopleComponents;
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
        internal ContactList(IEnumerable<Contact> contacts) => Contacts = contacts;

        public IEnumerable<Contact> Contacts { get; set; }
    }
}