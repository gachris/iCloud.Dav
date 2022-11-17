namespace iCloud.vCard.Net.Types;

/// <summary>Identifies the type of email address in a Person.</summary>
/// <seealso cref="Email" />
public enum EmailType
{
    /// <summary>Indicates a home email address type.</summary>
    Home = 1,
    /// <summary>Indicates a work email address type.</summary>
    Work = 2,
    /// <summary>Indicates an school email address type.</summary>
    School = 3,
    /// <summary>Indicates an other email address type.</summary>
    Other = 4,
    /// <summary>Indicates an internet unknown type.</summary>
    Custom = 5,
}