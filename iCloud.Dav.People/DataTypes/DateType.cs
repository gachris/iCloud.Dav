using System;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Enumerates the types of dates that can be used for a contact.
    /// </summary>
    public enum DateType
    {
        /// <summary>
        /// Indicates the anniversary type.
        /// </summary>
        Anniversary = 1,

        /// <summary>
        /// Indicates the other type.
        /// </summary>
        Other = 2,

        /// <summary>
        /// Indicates an unknown or custom type.
        /// </summary>
        Custom = 3
    }

    /// <summary>
    /// Enumerates the internal types of dates that can be used for a contact.
    /// </summary>
    [Flags]
    internal enum DateTypeInternal
    {
        /// <summary>
        /// Indicates the other type.
        /// </summary>
        Other = 1,

        /// <summary>
        /// Indicates the preferred type.
        /// </summary>
        Pref = 2
    }
}