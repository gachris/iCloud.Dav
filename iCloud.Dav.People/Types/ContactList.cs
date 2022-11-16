using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Serialization.Converters;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <summary>
/// Represents a strongly typed list of objects that can be accessed by index. Provides
/// methods to search, sort, and manipulate lists.
/// </summary>   
[TypeConverter(typeof(ContactListConverter))]
[XmlDeserializeType(typeof(MultiStatus))]
public class ContactList : List<Contact>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContactList"/> class that
    /// is empty and has the default initial capacity.
    /// </summary>
    public ContactList()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactList"/> class that
    /// is empty and has the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The number of elements that the new list can initially store.</param>
    /// <exception cref="System.ArgumentNullException">capacity is less than 0.</exception>
    public ContactList(int capacity) : base(capacity)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactList"/> class that
    /// contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new list.</param>
    /// <exception cref="System.ArgumentNullException"/>
    public ContactList(IEnumerable<Contact> collection) : base(collection)
    {
    }
}