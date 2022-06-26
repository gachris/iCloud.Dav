using System;

namespace iCloud.Dav.People
{
    /// <summary>The type of address.</summary>
    [Flags]
    public enum AddressTypes
    {
        /// <summary>Default address settings.</summary>
        Default = 0,
        /// <summary>A domestic delivery address.</summary>
        Domestic = 1,
        /// <summary>An international delivery address.</summary>
        International = 2,
        /// <summary>A postal delivery address.</summary>
        Postal = International | Domestic, // 0x00000003
        /// <summary>A parcel delivery address.</summary>
        Parcel = 4,
        /// <summary>A home delivery address.</summary>
        Home = Parcel | Domestic, // 0x00000005
        /// <summary>A work delivery address.</summary>
        Work = Parcel | International, // 0x00000006
    }
}
