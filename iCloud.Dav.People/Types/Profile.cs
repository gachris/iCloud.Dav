using iCloud.Dav.People.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <summary>
///     Social profile information for a <see cref="Profile" />.
/// </summary>
/// <seealso cref="Types.ProfileType" />
[Serializable]
[TypeConverter(typeof(ProfileConverter))]
public class Profile : ICloneable
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

    public object Clone() => MemberwiseClone();
}
