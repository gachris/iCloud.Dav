using System;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>Identifies the type of date in a vCard.</summary>
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

    /// <summary>Identifies the type of date in a vCard.</summary>
    /// <seealso cref="Date" />
    [Flags]
    internal enum DateTypeInternal
    {
        /// <summary>Indicates the other type.</summary>
        Other = 1,
        /// <summary>Indicates the pref type.</summary>
        Pref = 2
    }
}