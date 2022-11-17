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

internal class EmailConverter : TypeConverter
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
        if (!CanConvertTo(context, destinationType) || value is not Email email) throw GetConvertToException(value, destinationType);
        return Convert(email);
    }

    /// <summary>Converts the EMAIL property to email.</summary>
    private static Email Convert(IEnumerable<CardProperty> properties)
    {
        var emailAdressProperty = properties.FindByName(Constants.Contact.Property.EmailAddress.Property.EMAIL).ThrowIfNull(Constants.Contact.Property.EmailAddress.Property.EMAIL);
        var emailAddress = new Email { Address = emailAdressProperty.ToString() };

        _ = emailAdressProperty.Subproperties.TryParse<EmailTypeInternal>(out var emailTypeInternal);
        var isPreferred = emailTypeInternal.HasFlag(EmailTypeInternal.Pref);
        if (isPreferred)
        {
            emailAddress.IsPreferred = true;
            emailTypeInternal = emailTypeInternal.RemoveFlags(EmailTypeInternal.Pref);
        }

        var emailTypeFromInternal = EmailTypeMapping.GetType(emailTypeInternal);
        if (emailTypeFromInternal is 0)
        {
            var labelProperty = properties.FindByName(Constants.Contact.Property.EmailAddress.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Property.EmailAddress.CustomType.School:
                        emailTypeFromInternal = EmailType.School;
                        break;
                    default:
                        emailTypeFromInternal = EmailType.Custom;
                        emailAddress.Label = label;
                        break;
                }
            }
        }

        emailAddress.EmailType = emailTypeFromInternal;

        return emailAddress;
    }

    /// <summary>Converts the email to EMAIL property.</summary>
    private static IEnumerable<CardProperty> Convert(Email emailAddress)
    {
        var properties = new List<CardProperty>();
        emailAddress.Address = emailAddress.Address.ThrowIfNull(nameof(emailAddress.Address));
        var groupId = Guid.NewGuid().ToString();
        var emailProperty = new CardProperty(Constants.Contact.Property.EmailAddress.Property.EMAIL, emailAddress.Address, groupId);
        properties.Add(emailProperty);

        var emailTypeInternal = EmailTypeMapping.GetType(emailAddress.EmailType);
        if (emailAddress.IsPreferred)
        {
            emailTypeInternal = emailTypeInternal.AddFlags(EmailTypeInternal.Pref);
        }

        if (emailTypeInternal is not 0)
        {
            emailTypeInternal.StringArrayFlags()?
                .ForEach(type => emailProperty.Subproperties.Add(Constants.Contact.Property.EmailAddress.Property.TYPE, type.ToUpper()));
        }

        var label = emailAddress.EmailType switch
        {
            EmailType.School => Constants.Contact.Property.EmailAddress.CustomType.School,
            EmailType.Custom => emailAddress.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Property.EmailAddress.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return properties;
    }

}
