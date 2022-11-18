using iCloud.vCard.Net.Serialization;
using System;

namespace iCloud.vCard.Net.Data;

/// <summary>
///     An email address in a <see cref="Contact" />.
/// </summary>
/// <remarks>
///     Most Person email addresses are Internet email addresses.  However,
///     the Person specification allows other email address formats,
///     such as CompuServe and X400.  Unless otherwise specified, an
///     address is assumed to be an Internet address.
/// </remarks>
/// <seealso cref="Data.EmailType" />
[Serializable]
public class Email : CardDataType
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

    public Email()
    {
    }

    public Email(CardPropertyList properties)
    {
        var emailSerializer = new EmailSerializer();
        emailSerializer.Deserialize(properties, this);
    }
}
