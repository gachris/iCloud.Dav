using System;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Represents the type of an address that can be used for a contact.
    /// </summary>
    [Flags]
    public enum AddressType
    {
        /// <summary>
        /// Indicates a home address type.
        /// </summary>
        Home = 1,

        /// <summary>
        /// Indicates a work address type.
        /// </summary>
        Work = 2,

        /// <summary>
        /// Indicates a school address type.
        /// </summary>
        School = 3,

        /// <summary>
        /// Indicates an other address type.
        /// </summary>
        Other = 4,

        /// <summary>
        /// Indicates a custom address type.
        /// </summary>
        Custom = 5
    }

    /// <summary>
    /// Represents the internal type of an address that can be used for a contact.
    /// </summary>
    [Flags]
    internal enum AddressTypeInternal
    {
        /// <summary>
        /// Indicates a home address type.
        /// </summary>
        Home = 1,

        /// <summary>
        /// Indicates a work address type.
        /// </summary>
        Work = 2,

        /// <summary>
        /// Indicates an other address type.
        /// </summary>
        Other = 4,

        /// <summary>
        /// Indicates a preferred address type.
        /// </summary>
        Pref = 8
    }
}