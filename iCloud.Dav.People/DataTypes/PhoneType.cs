using System;

namespace iCloud.Dav.People.DataTypes
{
    public enum PhoneType
    {
        /// <summary>Indicates a mobile phone type.</summary>
        Mobile = 1,
        /// <summary>Indicates a iPhone phone type.</summary>
        iPhone = 2,
        /// <summary>Indicates an apple watch phone type.</summary>
        AppleWatch = 3,
        /// <summary>Indicates a home phone type</summary>
        Home = 4,
        /// <summary>Indicates a work phone type.</summary>
        Work = 5,
        /// <summary>Indicates an school phone type.</summary>
        School = 6,
        /// <summary>Indicates a main phone type.</summary>
        Main = 7,
        /// <summary>Indicates a cell phone type.</summary>
        HomeFax = 8,
        /// <summary>Indicates a fax phone type.</summary>
        WorkFax = 9,
        /// <summary>Indicates a pager phone type.</summary>
        Pager = 10,
        /// <summary>Indicates an other phone type.</summary>
        Other = 11,
        /// <summary>Indicates a voice phone type.</summary>
        Custom = 12
    }

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
}