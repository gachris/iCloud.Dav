using System;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.vCard.Net.Data;

[Serializable]
public class ContactGroup : Card
{
    #region Properties

    /// <summary>
    /// The name of the group.
    /// </summary>
    public virtual string? Name
    {
        get => Properties.Get<string>(Constants.ContactGroup.N);
        set => Properties.Set(Constants.ContactGroup.N, value);
    }

    /// <summary>
    /// The members of the ContactGroup.
    /// </summary>
    public virtual IReadOnlyCollection<string> Members => Get_Members();


    /// <summary>
    /// The kind of the ContactGroup.
    /// </summary>
    public string? Kind => Properties.Get<string>(Constants.ContactGroup.X_ADDRESSBOOKSERVER_KIND);

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactGroup" /> class.
    /// </summary>
    public ContactGroup() => Properties.Set(Constants.ContactGroup.X_ADDRESSBOOKSERVER_KIND, Constants.ContactGroup.GroupKind);

    #region Methods

    /// <summary>
    /// Adds member to the group.
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    public bool AddMember(string uniqueId)
    {
        var member_urn = string.Concat(Constants.ContactGroup.urn_Prefix, uniqueId);
        if (!Properties.Contains(member_urn))
        {
            Properties.Set(Constants.ContactGroup.X_ADDRESSBOOKSERVER_MEMBER, member_urn);
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
        var member_urn = string.Concat(Constants.ContactGroup.urn_Prefix, uniqueId);
        if (Properties.Contains(member_urn))
        {
            Properties.Remove(Properties.FindByName(member_urn)!);
            return true;
        }
        return false;
    }

    private List<string> Get_Members() => Properties.Where(x => x.Name == Constants.ContactGroup.X_ADDRESSBOOKSERVER_MEMBER)
        .Select(x => x.Value!.ToString()!.Replace(Constants.ContactGroup.urn_Prefix, null))
        .ToList();

    #endregion
}
