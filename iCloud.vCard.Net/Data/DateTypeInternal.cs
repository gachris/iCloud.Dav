using System;

namespace iCloud.vCard.Net.Data;

/// <summary>Identifies the type of date in a Person.</summary>
/// <seealso cref="Date" />
[Flags]
internal enum DateTypeInternal
{
    /// <summary>Indicates the other type.</summary>
    Other = 1,
    /// <summary>Indicates the pref type.</summary>
    Pref = 2
}
