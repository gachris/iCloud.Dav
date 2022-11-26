using System;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>The type of address.</summary>
    [Flags]
    public enum AddressTypeInternal
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