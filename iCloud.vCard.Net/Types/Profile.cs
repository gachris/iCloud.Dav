using iCloud.vCard.Net.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.vCard.Net.Types;

/// <summary>
///     Social profile information for a <see cref="Profile" />.
/// </summary>
/// <seealso cref="ProfileType" />
[Serializable]
[TypeConverter(typeof(ProfileConverter))]
public class Profile
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
}
