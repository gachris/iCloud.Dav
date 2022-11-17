using System;

namespace iCloud.vCard.Net.Types;

/// <summary>The type of a web site.</summary>
[Flags]
public enum WebsiteTypeInternal
{
    /// <summary>Indicates a home web site type.</summary>
    Home = 1,
    /// <summary>Indicates a work web site type.</summary>
    Work = 2,
    /// <summary>Indicates an other web site type.</summary>
    Other = 4,
    /// <summary>Indicates an pref web site type.</summary>
    Pref = 8,
}
