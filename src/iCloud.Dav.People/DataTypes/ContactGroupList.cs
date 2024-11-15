using System.ComponentModel;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.Serialization.Converters;
using iCloud.Dav.People.WebDav.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Represents a list of <see cref="ContactGroup"/> objects.
/// </summary>
[TypeConverter(typeof(ContactGroupListConverter))]
[XmlDeserializeType(typeof(MultiStatus))]
public class ContactGroupList
{
    /// <summary>
    /// Gets or sets the list of contact group.
    /// </summary>
    public virtual IList<ContactGroup> Items { get; set; }

    /// <summary>
    /// Gets or sets the type of the collection.
    /// </summary>
    /// <remarks>
    /// The value is always "groups".
    /// </remarks>
    public virtual string Kind { get; set; }
}