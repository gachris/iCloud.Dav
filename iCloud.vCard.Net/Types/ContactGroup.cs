using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace iCloud.vCard.Net.Types;

[Serializable]
public class ContactGroup : CardComponent
{
    #region Fields/Consts

    private readonly List<string> _members = new();

    #endregion

    #region Properties

    /// <summary>
    /// The name of the group.
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// The formatted name of the group.
    /// </summary>
    /// <remarks>
    ///     This property allows the name of the group to be
    ///     written in a manner specific to his or her culture.
    ///     The formatted name is not required to strictly
    ///     correspond with the family name, given name, etc.
    /// </remarks>
    public virtual string? FormattedName { get; set; }

    /// <summary>
    /// The member resource names of the ContactGroup.
    /// </summary>
    public virtual IReadOnlyCollection<string> Members { get; }

    /// <summary>
    /// The address book server of the ContactGroup.
    /// </summary>
    public virtual string? AddressBookServer { get; internal set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactGroup" /> class.
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
