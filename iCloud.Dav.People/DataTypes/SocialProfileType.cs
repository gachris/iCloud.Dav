using System;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Enumerates the types of social profiles that can be used for a contact.
/// </summary>
public enum SocialProfileType
{
    /// <summary>
    /// Indicates a Twitter social profile type.
    /// </summary>
    Twitter = 1,

    /// <summary>
    /// Indicates a Facebook social profile type.
    /// </summary>
    Facebook = 2,

    /// <summary>
    /// Indicates a LinkedIn social profile type.
    /// </summary>
    LinkedIn = 3,

    /// <summary>
    /// Indicates a Flickr social profile type.
    /// </summary>
    Flickr = 4,

    /// <summary>
    /// Indicates a MySpace social profile type.
    /// </summary>
    Myspace = 5,

    /// <summary>
    /// Indicates a Sina Weibo social profile type.
    /// </summary>
    SinaWeibo = 6,

    /// <summary>
    /// Indicates a custom or unknown social profile type.
    /// </summary>
    Custom = 7
}

/// <summary>
/// Enumerates the internal types of social profiles that can be used for a contact.
/// </summary>
[Flags]
internal enum SocialProfileTypeInternal
{
    /// <summary>
    /// Indicates a Twitter social profile type.
    /// </summary>
    Twitter = 1,

    /// <summary>
    /// Indicates a Facebook social profile type.
    /// </summary>
    Facebook = 2,

    /// <summary>
    /// Indicates a LinkedIn social profile type.
    /// </summary>
    LinkedIn = 4,

    /// <summary>
    /// Indicates a Flickr social profile type.
    /// </summary>
    Flickr = 8,

    /// <summary>
    /// Indicates a MySpace social profile type.
    /// </summary>
    Myspace = 16,

    /// <summary>
    /// Indicates a Sina Weibo social profile type.
    /// </summary>
    SinaWeibo = 32,

    /// <summary>
    /// Indicates a preferred social profile type.
    /// </summary>
    Pref = 64
}