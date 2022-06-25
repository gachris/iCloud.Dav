namespace iCloud.Dav.People
{
    /// <summary>The access classification of a Person.</summary>
    /// <remarks>
    ///     The access classification defines the intent of the Person owner.
    /// </remarks>
    public enum AccessClassification
    {
        /// <summary>The Person classification is unknown.</summary>
        Unknown,
        /// <summary>The Person is classified as public.</summary>
        Public,
        /// <summary>The Person is classified as private.</summary>
        Private,
        /// <summary>The Person is classified as confidential.</summary>
        Confidential,
    }
}
