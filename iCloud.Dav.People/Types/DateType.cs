namespace iCloud.Dav.People.Types;

/// <summary>Identifies the type of date in a Person.</summary>
/// <seealso cref="Date" />
public enum DateType
{
    /// <summary>Indicates the anniversary type.</summary>
    Anniversary = 1,
    /// <summary>Indicates the other type.</summary>
    Other = 2,
    /// <summary>Indicates an unknown type.</summary>
    Custom = 3,
}