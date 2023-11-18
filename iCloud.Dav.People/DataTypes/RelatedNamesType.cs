namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// Enumerates the types of related names that can be used for a contact.
/// </summary>
public enum RelatedNamesType
{
    /// <summary>
    /// Indicates a father related person type.
    /// </summary>
    Father = 1,

    /// <summary>
    /// Indicates a mother related person type.
    /// </summary>
    Mother = 2,

    /// <summary>
    /// Indicates a parent related person type.
    /// </summary>
    Parent = 3,

    /// <summary>
    /// Indicates a brother related person type.
    /// </summary>
    Brother = 4,

    /// <summary>
    /// Indicates a sister related person type.
    /// </summary>
    Sister = 5,

    /// <summary>
    /// Indicates a child related person type.
    /// </summary>
    Child = 6,

    /// <summary>
    /// Indicates a friend related person type.
    /// </summary>
    Friend = 7,

    /// <summary>
    /// Indicates a spouse related person type.
    /// </summary>
    Spouse = 8,

    /// <summary>
    /// Indicates a partner related person type.
    /// </summary>
    Partner = 9,

    /// <summary>
    /// Indicates an assistant related person type.
    /// </summary>
    Assistant = 10,

    /// <summary>
    /// Indicates a manager related person type.
    /// </summary>
    Manager = 11,

    /// <summary>
    /// Indicates an other related person type.
    /// </summary>
    Other = 12,

    /// <summary>
    /// Indicates a custom or unknown related person type.
    /// </summary>
    Custom = 13
}

/// <summary>
/// Enumerates the internal types of related names that can be used for a contact.
/// </summary>
internal enum RelatedNamesTypeInternal
{
    /// <summary>
    /// Indicates an other related person type.
    /// </summary>
    Other = 1,

    /// <summary>
    /// Indicates a preferred related person type.
    /// </summary>
    Pref = 2
}