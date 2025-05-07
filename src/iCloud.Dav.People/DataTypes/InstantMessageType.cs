namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Enumerates the types of instant messaging services that can be used for a contact.
/// </summary>
public enum InstantMessageType
{
    /// <summary>
    /// Indicates a home instant messaging type.
    /// </summary>
    Home = 1,

    /// <summary>
    /// Indicates a work instant messaging type.
    /// </summary>
    Work = 2,

    /// <summary>
    /// Indicates a facebook instant messaging type.
    /// </summary>
    Facebook = 3,

    /// <summary>
    /// Indicates an other instant messaging type.
    /// </summary>
    Other = 4,

    /// <summary>
    /// Indicates a custom instant messaging type.
    /// </summary>
    Custom = 5
}

/// <summary>
/// Enumerates the internal types of instant messaging services that can be used for a contact.
/// </summary>
[Flags]
internal enum InstantMessageTypeInternal
{
    /// <summary>
    /// Indicates a home instant messaging type.
    /// </summary>
    Home = 1,

    /// <summary>
    /// Indicates a work instant messaging type.
    /// </summary>
    Work = 2,

    /// <summary>
    /// Indicates a facebook instant messaging type.
    /// </summary>
    Facebook = 4,

    /// <summary>
    /// Indicates an other instant messaging type.
    /// </summary>
    Other = 8,

    /// <summary>
    /// Indicates a custom instant messaging type.
    /// </summary>
    Custom = 16,

    /// <summary>
    /// Indicates a preferred instant messaging type.
    /// </summary>
    Pref = 32
}