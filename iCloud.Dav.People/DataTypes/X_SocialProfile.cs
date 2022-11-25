using iCloud.Dav.People.Serialization.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
///     Social profile information for a <see cref="X_SocialProfile" />.
/// </summary>
public class X_SocialProfile : EncodableDataType
{
    /// <summary>The user name.</summary>
    public virtual string UserName { get; set; }

    /// <summary>The URL of the X-SOCIALPROFILE site.</summary>
    /// <remarks>The URL is not validated.</remarks>
    public virtual Uri Value { get; set; }

    /// <summary>The social profile types.</summary>
    public virtual IList<string> Types
    {
        get => Parameters.GetMany("TYPE");
        set => Parameters.Set("TYPE", value);
    }

    /// <summary>The url.</summary>
    public virtual string? Url { get; set; }

    public virtual bool IsPreferred { get; set; }

    public virtual string? Label { get; set; }

    /// <summary>The social profile type.</summary>
    public virtual ProfileType SocialProfileType { get; set; }

    public X_SocialProfile()
    {
    }

    public X_SocialProfile(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var serializer = new X_SocialProfileSerializer();
        CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
    }
}