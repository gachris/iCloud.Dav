namespace iCloud.Dav.People.Serialization;

/// <summary>
///     The encoding used to store a Card property value in text format.
/// </summary>
public enum CardEncoding
{
    /// <summary>Unknown or no encoding.</summary>
    Unknown,
    /// <summary>Standard escaped text.</summary>
    Escaped,
    /// <summary>Binary or BASE64 encoding.</summary>
    Base64,
    /// <summary>Quoted-Printable encoding.</summary>
    QuotedPrintable,
}
