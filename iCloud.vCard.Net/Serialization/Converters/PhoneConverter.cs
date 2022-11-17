using iCloud.vCard.Net.Serialization;
using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Types;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace iCloud.vCard.Net.Serialization.Converters;

internal class PhoneConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType.IsGenericType && sourceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) => (destinationType?.IsGenericType ?? false) && destinationType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (!CanConvertFrom(context, typeof(IEnumerable<CardProperty>)) || value is not IEnumerable<CardProperty> cardProperties) throw GetConvertFromException(value);
        return Convert(cardProperties);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (!CanConvertTo(context, destinationType) || value is not Phone phone) throw GetConvertToException(value, destinationType);
        return Convert(phone);
    }

    /// <summary>Converts the TEL property to phone.</summary>
    private static Phone Convert(IEnumerable<CardProperty> properties)
    {
        var telProperty = properties.FindByName(Constants.Contact.Property.Phone.Property.TEL).ThrowIfNull(Constants.Contact.Property.Phone.Property.TEL);
        var phone = new Phone { FullNumber = telProperty.ToString() };

        _ = telProperty.Subproperties.TryParse<PhoneTypeInternal>(out var phoneTypeInternal);
        var isPreferred = phoneTypeInternal.HasFlag(PhoneTypeInternal.Pref);
        if (isPreferred)
        {
            phone.IsPreferred = true;
            phoneTypeInternal = phoneTypeInternal.RemoveFlags(PhoneTypeInternal.Pref);
        }

        var phoneTypeFromInternal = PhoneTypeMapping.GetType(phoneTypeInternal);
        if (phoneTypeFromInternal is 0)
        {
            var labelProperty = properties.FindByName(Constants.Contact.Property.Phone.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Property.Phone.CustomType.AppleWatch:
                        phoneTypeFromInternal = PhoneType.AppleWatch;
                        break;
                    case Constants.Contact.Property.Phone.CustomType.School:
                        phoneTypeFromInternal = PhoneType.School;
                        break;
                    default:
                        phoneTypeFromInternal = PhoneType.Custom;
                        phone.Label = label;
                        break;
                }
            }
        }

        phone.PhoneType = phoneTypeFromInternal;

        return phone;
    }

    /// <summary>Converts the phone to TEL property.</summary>
    private static IEnumerable<CardProperty> Convert(Phone phone)
    {
        var properties = new List<CardProperty>();
        phone.FullNumber = phone.FullNumber.ThrowIfNull(nameof(phone.FullNumber));
        var groupId = Guid.NewGuid().ToString();
        var telProperty = new CardProperty(Constants.Contact.Property.Phone.Property.TEL, phone.FullNumber, groupId);
        properties.Add(telProperty);

        var phoneTypeInternal = PhoneTypeMapping.GetType(phone.PhoneType);
        if (phone.IsPreferred)
        {
            phoneTypeInternal = phoneTypeInternal.AddFlags(PhoneTypeInternal.Pref);
        }

        if (phoneTypeInternal is not 0)
        {
            phoneTypeInternal.StringArrayFlags()?.
                ForEach(type => telProperty.Subproperties.Add(Constants.Contact.Property.Phone.Property.TYPE, type.ToUpper()));
        }

        var label = phone.PhoneType switch
        {
            PhoneType.AppleWatch => Constants.Contact.Property.Phone.CustomType.AppleWatch,
            PhoneType.School => Constants.Contact.Property.Phone.CustomType.School,
            PhoneType.Custom => phone.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Property.Phone.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return properties;
    }
}