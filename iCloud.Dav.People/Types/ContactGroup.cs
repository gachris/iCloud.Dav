using iCloud.Dav.Core;
using iCloud.Dav.People.Serialization.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

[Serializable]
[TypeConverter(typeof(ContactGroupConverter))]
public class ContactGroup : IDirectResponseSchema
{
    #region Fields/Consts

    private readonly List<string> _members = new();

    #endregion

    #region Properties

    /// <summary>A value that uniquely identifies the Person.</summary>
    /// <remarks>
    ///     This value is optional.  The string must be any string
    ///     that can be used to uniquely identify the Person.  The
    ///     usage of the field is determined by the software.  Typical
    ///     possibilities for a unique string include a URL, a GUID,
    ///     or an LDAP directory path.  However, there is no particular
    ///     standard dictated by the Person specification.
    /// </remarks>
    public virtual string? UniqueId { get; set; }

    /// <summary>The name of the group.</summary>
    public virtual string? Name { get; set; }

    /// <summary>The formatted name of the group.</summary>
    /// <remarks>
    ///     This property allows the name of the group to be
    ///     written in a manner specific to his or her culture.
    ///     The formatted name is not required to strictly
    ///     correspond with the family name, given name, etc.
    /// </remarks>
    public virtual string? FormattedName { get; set; }

    /// <summary>The name of the product that generated the group.</summary>
    public virtual string? ProductId { get; set; }

    /// <summary>The revision date of the group.</summary>
    /// <remarks>
    ///     The revision date is not automatically updated by the
    ///     group when modifying properties.  It is up to the
    ///     developer to change the revision date as needed.
    /// </remarks>
    public virtual DateTime? RevisionDate { get; set; }

    /// <summary>
    /// The member resource names of the ContactGroup.
    /// </summary>
    public virtual IReadOnlyCollection<string> Members { get; }

    /// <summary>The e-tag of the person.</summary>
    /// <remarks>
    /// Will be set by the service deserialization method,
    /// or the by json response parser if implemented on service.
    /// </remarks>
    public virtual string? ETag { get; set; }

    /// <summary>
    /// The address book server of the ContactGroup.
    /// </summary>
    public virtual string? AddressBookServer { get; internal set; }

    /// <summary>
    /// The Url of the ContactGroup.
    /// </summary>
    public virtual string? Url { get; internal set; }

    #endregion

    /// <summary>
    ///     Initializes a new instance of the <see cref="ContactGroup" /> class.
    /// </summary>
    public ContactGroup() => Members = new ReadOnlyCollection<string>(_members);

    #region Methods

    /// <summary>
    /// Adds member to the group.
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    public bool AddMember(string uniqueId)
    {
        if (!_members.Contains(uniqueId))
        {
            _members.Add(uniqueId);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes member from the group.
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    public bool RemoveMember(string uniqueId)
    {
        if (_members.Contains(uniqueId))
        {
            _members.Remove(uniqueId);
            return true;
        }
        return false;
    }

    #endregion
}
