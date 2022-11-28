using System;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>The type of address.</summary>
    [Flags]
    public enum AddressType
    {
        /// <summary>Home address type.</summary>
        Home = 1,
        /// <summary>Work address type.</summary>
        Work = 2,
        /// <summary>Work address type.</summary>
        School = 3,
        /// <summary>Other address type.</summary>
        Other = 4,
        /// <summary>Unknown address type.</summary>
        Custom = 5
    }

    /// <summary>The type of address.</summary>
    [Flags]
    internal enum AddressTypeInternal
    {
        /// <summary>Home address type.</summary>
        Home = 1,
        /// <summary>Work address type.</summary>
        Work = 2,
        /// <summary>Other address type.</summary>
        Other = 4,
        /// <summary>Pref address type.</summary>
        Pref = 8
    }
}