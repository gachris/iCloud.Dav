using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;

namespace iCloud.vCard.Net.Serialization;

public class AddressSerializer : SerializerBase<Address>
{
    /// <summary>Converts the ADR property to address.</summary>
    public override void Deserialize(CardPropertyList properties, Address address)
    {
        const string name = Constants.Contact.Address.Property.ADR;

        var property = properties.FindByName(name).ThrowIfNull(name);
        var values = property.ToString().Split(';');

        if (values.Length >= 7)
            address.Country = values[6].Trim();
        if (values.Length >= 6)
            address.PostalCode = values[5].Trim();
        if (values.Length >= 5)
            address.Region = values[4].Trim();
        if (values.Length >= 4)
            address.City = values[3].Trim();
        if (values.Length >= 3)
            address.Street = values[2].Trim();

        _ = property.Subproperties.TryParse<AddressTypeInternal>(out var addressTypeInternal);

        var isPreferred = addressTypeInternal.HasFlag(AddressTypeInternal.Pref);
        if (isPreferred)
        {
            address.IsPreferred = true;
            addressTypeInternal = addressTypeInternal.RemoveFlags(AddressTypeInternal.Pref);
        }

        var addressTypeFromInternal = AddressTypeMapping.GetType(addressTypeInternal);
        if (addressTypeFromInternal is 0)
        {
            var labelProperty = properties.FindByName(Constants.Contact.Address.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Address.CustomType.School:
                        addressTypeFromInternal = AddressType.School;
                        break;
                    default:
                        addressTypeFromInternal = AddressType.Custom;
                        address.Label = label;
                        break;
                }
            }
        }

        address.AddressType = addressTypeFromInternal;
        address.CountryCode = properties.FindByName(Constants.Contact.Address.Property.X_ABADR)?.Value?.ToString();
    }

    /// <summary>Converts the address to ADR property.</summary>
    public override CardPropertyList Serialize(Address address)
    {
        var properties = new List<CardProperty>();
        if (IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address), "Cannot be empty!");
        var valueCollection = new ValueCollection(';')
        {
            string.Empty,
            string.Empty,
            address.Street,
            address.City,
            address.Region,
            address.PostalCode,
            address.Country
        };
        var groupId = Guid.NewGuid().ToString();
        var addressProperty = new CardProperty(Constants.Contact.Address.Property.ADR, valueCollection)
        {
            Group = groupId
        };
        properties.Add(addressProperty);

        var addressTypeInternal = AddressTypeMapping.GetType(address.AddressType);
        if (address.IsPreferred)
        {
            addressTypeInternal = addressTypeInternal.AddFlags(AddressTypeInternal.Pref);
        }

        if (addressTypeInternal is not 0)
        {
            addressTypeInternal.StringArrayFlags()?.
                ForEach(type => addressProperty.Subproperties.Add(Constants.Contact.Address.Property.TYPE, type.ToUpper()));
        }

        var label = address.AddressType switch
        {
            AddressType.School => Constants.Contact.Address.CustomType.School,
            AddressType.Custom => address.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Address.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        if (!string.IsNullOrEmpty(address.CountryCode))
        {
            var abAdrProperty = new CardProperty(Constants.Contact.Address.Property.X_ABADR, address.CountryCode, groupId);
            properties.Add(abAdrProperty);
        }

        return new(properties);
    }

    private static bool IsNullOrEmpty(Address? address)
    {
        if (string.IsNullOrEmpty(address?.City)
         && string.IsNullOrEmpty(address?.Country)
         && string.IsNullOrEmpty(address?.PostalCode)
         && string.IsNullOrEmpty(address?.Region)
         && string.IsNullOrEmpty(address?.Street))
            return true;
        return false;
    }
}
