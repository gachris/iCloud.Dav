using iCloud.Dav.People.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <summary>
///     An email address in a <see cref="Person" />.
/// </summary>
/// <remarks>
///     Most Person email addresses are Internet email addresses.  However,
///     the Person specification allows other email address formats,
///     such as CompuServe and X400.  Unless otherwise specified, an
///     address is assumed to be an Internet address.
/// </remarks>
/// <seealso cref="Types.EmailType" />
[Serializable]
[TypeConverter(typeof(EmailConverter))]
public class Email : ICloneable
{
    #region Properties

    /// <summary>The email address.</summary>
    /// <remarks>
    ///     The format of the email address is not validated by the class.
    /// </remarks>
    public virtual string? Address { get; set; }

    /// <summary>The email address type.</summary>
    public virtual EmailType EmailType { get; set; }

    public virtual bool IsPreferred { get; set; }

    public virtual string? Label { get; set; }

    #endregion

    public object Clone() => MemberwiseClone();
}
