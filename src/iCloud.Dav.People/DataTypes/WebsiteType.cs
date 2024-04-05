using System;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Enumerates the types of websites that can be used for a contact.
/// </summary>
public enum WebsiteType
{
    /// <summary>
    /// Indicates a homepage website type.
    /// </summary>
    HomePage = 1,

    /// <summary>
    /// Indicates a home website type.
    /// </summary>
    Home = 2,

    /// <summary>
    /// Indicates a work website type.
    /// </summary>
    Work = 3,

    /// <summary>
    /// Indicates a school website type.
    /// </summary>
    School = 4,

    /// <summary>
    /// Indicates a blog website type.
    /// </summary>
    Blog = 5,

    /// <summary>
    /// Indicates an other website type.
    /// </summary>
    Other = 6,

    /// <summary>
    /// Indicates a custom website type.
    /// </summary>
    Custom = 7
}

/// <summary>
/// Enumerates the internal types of websites that can be used for a contact.
/// </summary>
[Flags]
internal enum WebsiteTypeInternal
{
    /// <summary>
    /// Indicates a home website type.
    /// </summary>
    Home = 1,

    /// <summary>
    /// Indicates a work website type.
    /// </summary>
    Work = 2,

    /// <summary>
    /// Indicates an other website type.
    /// </summary>
    Other = 4,

    /// <summary>
    /// Indicates a preferred website type.
    /// </summary>
    Pref = 8
}