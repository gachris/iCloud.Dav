using System;

namespace iCloud.vCard.Net.Types;

/// <summary>Identifies the type of email address in a Person.</summary>
/// <seealso cref="Email" />
[Flags]
internal enum EmailTypeInternal
{
    /// <summary>Indicates an internet email address type.</summary>
    Internet = 1,
    /// <summary>Indicates a home email address type.</summary>
    Home = 2,
    /// <summary>Indicates a work email address type.</summary>
    Work = 4,
    /// <summary>Indicates an other email address type.</summary>
    Other = 8,
    /// <summary>Indicates an other pref address type.</summary>
    Pref = 16
}