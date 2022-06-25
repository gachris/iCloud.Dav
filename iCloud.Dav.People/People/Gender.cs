namespace iCloud.Dav.People
{
    /// <summary>The gender (male or female) of the contact.</summary>
    /// <remarks>
    ///     <para>
    ///         Gender is not directly supported by the Person specification.
    ///         It is recognized by Microsoft Outlook and the Windows Address
    ///         Book through an extended property called X-WAB-GENDER.  This
    ///         property has a value of 1 for women and 2 for men.
    ///     </para>
    /// </remarks>
    /// <seealso cref="Gender" />
    public enum Gender
    {
        /// <summary>Unknown gender.</summary>
        Unknown,
        /// <summary>Female gender.</summary>
        Female,
        /// <summary>Male gender.</summary>
        Male,
    }
}
