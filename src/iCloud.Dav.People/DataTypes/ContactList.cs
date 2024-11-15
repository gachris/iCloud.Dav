using System.ComponentModel;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.Serialization.Converters;
using iCloud.Dav.People.WebDav.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a list of <see cref="Contact"/> objects.
/// </summary>
[TypeConverter(typeof(ContactListConverter))]
[XmlDeserializeType(typeof(MultiStatus))]
public class ContactList
{
    /// <summary>
    /// Gets or sets the list of contacts.
    /// </summary>
    public virtual IList<Contact> Items { get; set; }

    /// <summary>
    /// Gets or sets the type of the collection.
    /// </summary>
    /// <remarks>
    /// The value is always "contacts".
    /// </remarks>
    public virtual string Kind { get; set; }
}