using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.Serialization.Mapping;
using iCloud.Dav.People.Types;
using iCloud.Dav.People.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace iCloud.Dav.People.Serialization.Converters;

internal class AddressConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType.IsGenericType && sourceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) =>
        (destinationType?.IsGenericType ?? false) && destinationType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (!CanConvertFrom(context, typeof(IEnumerable<CardProperty>)) || value is not IEnumerable<CardProperty> cardProperties) throw GetConvertFromException(value);
        return Convert(cardProperties);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (!CanConvertTo(context, destinationType) || value is not Address address) throw GetConvertToException(value, destinationType);
        return Convert(address);
    }

    /// <summary>Converts the ADR property to address.</summary>
    private static Address? Convert(IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.Address.Property.ADR).ThrowIfNull(Constants.Contact.Property.Address.Property.ADR);
        var values = property.ToString().Split(';');

        var address = new Address();
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
            var labelProperty = properties.FindByName(Constants.Contact.Property.Address.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Property.Address.CustomType.School:
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
        address.CountryCode = properties.FindByName(Constants.Contact.Property.Address.Property.X_ABADR)?.Value?.ToString();

        return address;
    }

    /// <summary>Converts the address to ADR property.</summary>
    private static IEnumerable<CardProperty> Convert(Address address)
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
        var addressProperty = new CardProperty(Constants.Contact.Property.Address.Property.ADR, valueCollection)
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
                ForEach(type => addressProperty.Subproperties.Add(Constants.Contact.Property.Address.Property.TYPE, type.ToUpper()));
        }

        var label = address.AddressType switch
        {
            AddressType.School => Constants.Contact.Property.Address.CustomType.School,
            AddressType.Custom => address.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Property.Address.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        if (!string.IsNullOrEmpty(address.CountryCode))
        {
            var abAdrProperty = new CardProperty(Constants.Contact.Property.Address.Property.X_ABADR, address.CountryCode, groupId);
            properties.Add(abAdrProperty);
        }

        return properties;
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
