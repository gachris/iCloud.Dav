using System;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Enumerates the types of email addresses that can be used in a person's contact information.
    /// </summary>
    public enum EmailType
    {
        /// <summary>
        /// Indicates a home email address type.
        /// </summary>
        Home = 1,

        /// <summary>
        /// Indicates a work email address type.
        /// </summary>
        Work = 2,

        /// <summary>
        /// Indicates a school email address type.
        /// </summary>
        School = 3,

        /// <summary>
        /// Indicates an other email address type.
        /// </summary>
        Other = 4,

        /// <summary>
        /// Indicates an unknown or custom email address type.
        /// </summary>
        Custom = 5
    }

    /// <summary>
    /// Enumerates the internal types of email addresses that can be used in a person's contact information.
    /// </summary>
    [Flags]
    internal enum EmailTypeInternal
    {
        /// <summary>
        /// Indicates an internet email address type.
        /// </summary>
        Internet = 1,

        /// <summary>
        /// Indicates a home email address type.
        /// </summary>
        Home = 2,

        /// <summary>
        /// Indicates a work email address type.
        /// </summary>
        Work = 4,

        /// <summary>
        /// Indicates an other email address type.
        /// </summary>
        Other = 8,

        /// <summary>
        /// Indicates a preferred email address type.
        /// </summary>
        Pref = 16
    }
}