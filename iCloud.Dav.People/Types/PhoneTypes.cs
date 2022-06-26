using System;

namespace iCloud.Dav.People
{
    /// <summary>
    ///     Identifies different phone types (e.g. Fax, BBS, etc).
    /// </summary>
    /// <seealso cref="Phone" />
    [Flags]
    public enum PhoneTypes
    {
        /// <summary>Indicates default properties.</summary>
        Default = 0,
        /// <summary>Indicates a bulletin board system.</summary>
        BBS = 1,
        /// <summary>Indicates a car phone.</summary>
        Car = 2,
        /// <summary>Indicates a cell phone.</summary>
        Cellular = 4,
        /// <summary>Indicates a celluar voice number.</summary>
        CellularVoice = 2052, // 0x00000804
        /// <summary>Indicates a facsimile number.</summary>
        Fax = 8,
        /// <summary>Indicates a home number</summary>
        Home = 16, // 0x00000010
        /// <summary>Indicates a home and voice number.</summary>
        HomeVoice = 2064, // 0x00000810
        /// <summary>Indicates an ISDN number.</summary>
        ISDN = 32, // 0x00000020
        /// <summary>Indicates a messaging service on the number.</summary>
        MessagingService = 64, // 0x00000040
        /// <summary>Indicates a MODEM number.</summary>
        Modem = 128, // 0x00000080
        /// <summary>Indicates a pager number.</summary>
        Pager = 256, // 0x00000100
        /// <summary>Indicates a preferred number.</summary>
        Preferred = 512, // 0x00000200
        /// <summary>Indicates a video number.</summary>
        Video = 1024, // 0x00000400
        /// <summary>Indicates a voice number.</summary>
        Voice = 2048, // 0x00000800
        /// <summary>Indicates a work number.</summary>
        Work = 4096, // 0x00001000
        /// <summary>Indicates a work fax number.</summary>
        WorkFax = Work | Fax, // 0x00001008
        /// <summary>Indicates a work and voice number.</summary>
        WorkVoice = Work | Voice, // 0x00001800
    }
}
