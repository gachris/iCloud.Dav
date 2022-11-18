using iCloud.vCard.Net.Serialization;
using System;

namespace iCloud.vCard.Net.Data;

/// <summary>
///     Phone information for a <see cref="Contact" />.
/// </summary>
/// <seealso cref="Data.PhoneType" />
[Serializable]
public class Phone : CardDataType
{
    #region Properties

    /// <summary>The full telephone number.</summary>
    public virtual string? FullNumber { get; set; }

    /// <summary>The phone type.</summary>
    public virtual PhoneType PhoneType { get; set; }

    public virtual string? Label { get; set; }

    public virtual bool IsPreferred { get; set; }

    #endregion

    public Phone()
    {
    }

    public Phone(CardPropertyList properties)
    {
        var phoneSerializer = new PhoneSerializer();
        phoneSerializer.Deserialize(properties, this);
    }
}