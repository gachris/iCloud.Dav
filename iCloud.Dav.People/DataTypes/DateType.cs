namespace iCloud.Dav.People.DataTypes
{
    /// <summary>Identifies the type of date in a vCard.</summary>
    /// <seealso cref="X_ABDate" />
    public enum DateType
    {
        /// <summary>Indicates the anniversary type.</summary>
        Anniversary = 1,
        /// <summary>Indicates the other type.</summary>
        Other = 2,
        /// <summary>Indicates an unknown type.</summary>
        Custom = 3,
    }
}