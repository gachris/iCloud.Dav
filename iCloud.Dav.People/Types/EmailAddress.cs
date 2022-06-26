namespace iCloud.Dav.People
{
    /// <summary>
    ///     An email address in a <see cref="Person" />.
    /// </summary>
    /// <remarks>
    ///     Most Person email addresses are Internet email addresses.  However,
    ///     the Person specification allows other email address formats,
    ///     such as CompuServe and X400.  Unless otherwise specified, an
    ///     address is assumed to be an Internet address.
    /// </remarks>
    /// <seealso cref="EmailAddressType" />
    public class EmailAddress
    {
        #region Properties

        /// <summary>The email address.</summary>
        /// <remarks>
        ///     The format of the email address is not validated by the class.
        /// </remarks>
        public virtual string Address { get; set; }

        /// <summary>The email address type.</summary>
        public virtual EmailAddressType EmailType { get; set; }

        /// <summary>
        ///     Indicates a preferred (top priority) email address.
        /// </summary>
        public virtual bool IsPreferred { get; set; } 

        #endregion
    }
}
