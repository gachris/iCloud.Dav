using iCloud.vCard.Net.Serialization;
using System;

namespace iCloud.vCard.Net.Data;

/// <summary>
///     Social profile information for a <see cref="Profile" />.
/// </summary>
/// <seealso cref="ProfileType" />
[Serializable]
public class Profile : CardDataType
{
    #region Properties

    /// <summary>The user name.</summary>
    public virtual string? UserName { get; set; }

    /// <summary>The url.</summary>
    public virtual string? Url { get; set; }

    public virtual bool IsPreferred { get; set; }

    public virtual string? Label { get; set; }

    /// <summary>The social profile type.</summary>
    public virtual ProfileType SocialProfileType { get; set; }

    #endregion

    public Profile()
    {
    }

    public Profile(CardPropertyList properties)
    {
        var profileSerializer = new ProfileSerializer();
        profileSerializer.Deserialize(properties, this);
    }
}
