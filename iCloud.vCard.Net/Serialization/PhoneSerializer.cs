using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;

namespace iCloud.vCard.Net.Serialization;

public class PhoneSerializer : SerializerBase<Phone>
{
    /// <summary>Converts the TEL property to phone.</summary>
    public override void Deserialize(CardPropertyList properties, Phone phone)
    {
        var telProperty = properties.FindByName(Constants.Contact.Phone.Property.TEL).ThrowIfNull(Constants.Contact.Phone.Property.TEL);
        phone.FullNumber = telProperty.ToString();

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
            var labelProperty = properties.FindByName(Constants.Contact.Phone.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Phone.CustomType.AppleWatch:
                        phoneTypeFromInternal = PhoneType.AppleWatch;
                        break;
                    case Constants.Contact.Phone.CustomType.School:
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
    }

    /// <summary>Converts the phone to TEL property.</summary>
    public override CardPropertyList Serialize(Phone phone)
    {
        var properties = new List<CardProperty>();
        phone.FullNumber = phone.FullNumber.ThrowIfNull(nameof(phone.FullNumber));
        var groupId = Guid.NewGuid().ToString();
        var telProperty = new CardProperty(Constants.Contact.Phone.Property.TEL, phone.FullNumber, groupId);
        properties.Add(telProperty);

        var phoneTypeInternal = PhoneTypeMapping.GetType(phone.PhoneType);
        if (phone.IsPreferred)
        {
            phoneTypeInternal = phoneTypeInternal.AddFlags(PhoneTypeInternal.Pref);
        }

        if (phoneTypeInternal is not 0)
        {
            phoneTypeInternal.StringArrayFlags()?.
                ForEach(type => telProperty.Subproperties.Add(Constants.Contact.Phone.Property.TYPE, type.ToUpper()));
        }

        var label = phone.PhoneType switch
        {
            PhoneType.AppleWatch => Constants.Contact.Phone.CustomType.AppleWatch,
            PhoneType.School => Constants.Contact.Phone.CustomType.School,
            PhoneType.Custom => phone.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Phone.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return new(properties);
    }
}
