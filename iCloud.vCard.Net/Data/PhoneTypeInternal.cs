using System;

namespace iCloud.vCard.Net.Data;

/// <summary>
///     Identifies different phone types (e.g. Main, Pager, etc).
/// </summary>
/// <seealso cref="Phone" />
[Flags]
internal enum PhoneTypeInternal
{
    /// <summary>Indicates a iPhone phone type.</summary>
    iPhone = 1,
    /// <summary>Indicates a main phone type.</summary>
    Main = 2,
    /// <summary>Indicates a cell phone type.</summary>
    Cell = 4,
    /// <summary>Indicates a fax phone type.</summary>
    Fax = 8,
    /// <summary>Indicates a home phone type</summary>
    Home = 16,
    /// <summary>Indicates a pager phone type.</summary>
    Pager = 32,
    /// <summary>Indicates a voice phone type.</summary>
    Voice = 64,
    /// <summary>Indicates a work phone type.</summary>
    Work = 128,
    /// <summary>Indicates an other phone type.</summary>
    Other = 256,
    /// <summary>Indicates an other phone type.</summary>
    Pref = 512,
}
