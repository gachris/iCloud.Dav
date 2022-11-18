using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;

namespace iCloud.vCard.Net.Serialization;

public class EmailSerializer : SerializerBase<Email>
{
    /// <summary>Converts the EMAIL property to email.</summary>
    public override void Deserialize(CardPropertyList properties, Email emailAddress)
    {
        var emailAdressProperty = properties.FindByName(Constants.Contact.EmailAddress.Property.EMAIL).ThrowIfNull(Constants.Contact.EmailAddress.Property.EMAIL);
        emailAddress.Address = emailAdressProperty.ToString();

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
            var labelProperty = properties.FindByName(Constants.Contact.EmailAddress.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.EmailAddress.CustomType.School:
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
    }

    /// <summary>Converts the email to EMAIL property.</summary>
    public override CardPropertyList Serialize(Email emailAddress)
    {
        var properties = new List<CardProperty>();
        emailAddress.Address = emailAddress.Address.ThrowIfNull(nameof(emailAddress.Address));
        var groupId = Guid.NewGuid().ToString();
        var emailProperty = new CardProperty(Constants.Contact.EmailAddress.Property.EMAIL, emailAddress.Address, groupId);
        properties.Add(emailProperty);

        var emailTypeInternal = EmailTypeMapping.GetType(emailAddress.EmailType);
        if (emailAddress.IsPreferred)
        {
            emailTypeInternal = emailTypeInternal.AddFlags(EmailTypeInternal.Pref);
        }

        if (emailTypeInternal is not 0)
        {
            emailTypeInternal.StringArrayFlags()?
                .ForEach(type => emailProperty.Subproperties.Add(Constants.Contact.EmailAddress.Property.TYPE, type.ToUpper()));
        }

        var label = emailAddress.EmailType switch
        {
            EmailType.School => Constants.Contact.EmailAddress.CustomType.School,
            EmailType.Custom => emailAddress.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.EmailAddress.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return new(properties);
    }
}
