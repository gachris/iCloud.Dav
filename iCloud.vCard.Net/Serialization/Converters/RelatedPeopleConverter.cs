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

internal class RelatedPeopleConverter : TypeConverter
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
        if (!CanConvertTo(context, destinationType) || value is not RelatedPeople relatedPerson) throw GetConvertToException(value, destinationType);
        return Convert(relatedPerson);
    }

    /// <summary>Converts the X-ABRELATEDNAMES property to related person.</summary>
    private static RelatedPeople? Convert(IEnumerable<CardProperty> properties)
    {
        var relatedNamesProperty = properties.FindByName(Constants.Contact.Property.RelatedPerson.Property.X_ABRELATEDNAMES).ThrowIfNull(Constants.Contact.Property.RelatedPerson.Property.X_ABRELATEDNAMES);
        var relatedPerson = new RelatedPeople { Name = relatedNamesProperty.ToString() };

        _ = relatedNamesProperty.Subproperties.TryParse<RelatedPeopleTypeInternal>(out var relatedPersonTypeInternal);
        var isPreferred = relatedPersonTypeInternal.HasFlag(RelatedPeopleTypeInternal.Pref);
        if (isPreferred)
        {
            relatedPerson.IsPreferred = true;
            relatedPersonTypeInternal = relatedPersonTypeInternal.RemoveFlags(RelatedPeopleTypeInternal.Pref);
        }

        var relatedPersonTypeFromInternal = RelatedPeopleTypeMapping.GetType(relatedPersonTypeInternal);
        if (relatedPersonTypeFromInternal is 0)
        {
            var labelProperty = properties.FindByName(Constants.Contact.Property.RelatedPerson.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.Property.RelatedPerson.CustomType.Father:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Father;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Mother:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Mother;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Parent:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Parent;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Brother:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Brother;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Sister:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Sister;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Child:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Child;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Friend:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Friend;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Spouse:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Spouse;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Partner:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Partner;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Assistant:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Assistant;
                        break;
                    case Constants.Contact.Property.RelatedPerson.CustomType.Manager:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Manager;
                        break;
                    default:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Custom;
                        relatedPerson.Label = label;
                        break;
                }
            }
        }

        relatedPerson.RelatedPersonType = relatedPersonTypeFromInternal;

        return relatedPerson;
    }

    /// <summary>Converts the related person to X-ABRELATEDNAMES property.</summary>
    private static IEnumerable<CardProperty> Convert(RelatedPeople relatedPerson)
    {
        var properties = new List<CardProperty>();
        relatedPerson.Name = relatedPerson.Name.ThrowIfNull(nameof(relatedPerson.Name));
        var groupId = Guid.NewGuid().ToString();
        var urlProperty = new CardProperty(Constants.Contact.Property.RelatedPerson.Property.X_ABRELATEDNAMES, relatedPerson.Name.ToString(), groupId);
        properties.Add(urlProperty);

        var relatedPersonTypeInternal = RelatedPeopleTypeMapping.GetType(relatedPerson.RelatedPersonType);
        if (relatedPerson.IsPreferred)
        {
            relatedPersonTypeInternal = relatedPersonTypeInternal.AddFlags(RelatedPeopleTypeInternal.Pref);
        }

        if (relatedPersonTypeInternal is not 0)
        {
            relatedPersonTypeInternal.StringArrayFlags()?.
                ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Property.RelatedPerson.Property.TYPE, type.ToUpper()));
        }

        var label = relatedPerson.RelatedPersonType switch
        {
            RelatedPeopleType.Father => Constants.Contact.Property.RelatedPerson.CustomType.Father,
            RelatedPeopleType.Mother => Constants.Contact.Property.RelatedPerson.CustomType.Mother,
            RelatedPeopleType.Parent => Constants.Contact.Property.RelatedPerson.CustomType.Parent,
            RelatedPeopleType.Brother => Constants.Contact.Property.RelatedPerson.CustomType.Brother,
            RelatedPeopleType.Sister => Constants.Contact.Property.RelatedPerson.CustomType.Sister,
            RelatedPeopleType.Child => Constants.Contact.Property.RelatedPerson.CustomType.Child,
            RelatedPeopleType.Friend => Constants.Contact.Property.RelatedPerson.CustomType.Friend,
            RelatedPeopleType.Spouse => Constants.Contact.Property.RelatedPerson.CustomType.Spouse,
            RelatedPeopleType.Partner => Constants.Contact.Property.RelatedPerson.CustomType.Partner,
            RelatedPeopleType.Assistant => Constants.Contact.Property.RelatedPerson.CustomType.Assistant,
            RelatedPeopleType.Manager => Constants.Contact.Property.RelatedPerson.CustomType.Manager,
            RelatedPeopleType.Custom => relatedPerson.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Property.RelatedPerson.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return properties;
    }
}
