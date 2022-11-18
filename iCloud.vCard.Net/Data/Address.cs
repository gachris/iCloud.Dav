using iCloud.vCard.Net.Serialization;
using System;

namespace iCloud.vCard.Net.Data;

/// <summary>A postal address.</summary>
[Serializable]
public class Address : CardDataType
{
    #region Properties

    /// <summary>
    /// The type of postal address.
    /// </summary>
    public virtual AddressType AddressType { get; set; }

    /// <summary>
    /// The city or locality of the address.
    /// </summary>
    public virtual string? City { get; set; }

    /// <summary>
    /// The country name of the address.
    /// </summary>
    public virtual string? Country { get; set; }

    /// <summary>
    /// The postal code (e.g. ZIP code) of the address.
    /// </summary>
    public virtual string? PostalCode { get; set; }

    /// <summary>
    /// The region (state or province) of the address.
    /// </summary>
    public virtual string? Region { get; set; }

    /// <summary>
    /// The street of the delivery address.
    /// </summary>
    public virtual string? Street { get; set; }

    public virtual bool IsPreferred { get; set; }

    public virtual string? Label { get; set; }

    public virtual string? CountryCode { get; set; }

    #endregion

    public Address()
    {
    }

    public Address(CardPropertyList properties)
    {
        var addressSerializer = new AddressSerializer();
        addressSerializer.Deserialize(properties, this);
    }
}
