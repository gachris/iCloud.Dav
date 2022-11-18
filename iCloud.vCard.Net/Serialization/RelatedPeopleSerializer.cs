using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;

namespace iCloud.vCard.Net.Serialization;

public class RelatedPeopleSerializer : SerializerBase<RelatedPeople>
{
    /// <summary>Converts the X-ABRELATEDNAMES property to related person.</summary>
    public override void Deserialize(CardPropertyList properties, RelatedPeople relatedPerson)
    {
        var relatedNamesProperty = properties.FindByName(Constants.Contact.RelatedPerson.Property.X_ABRELATEDNAMES).ThrowIfNull(Constants.Contact.RelatedPerson.Property.X_ABRELATEDNAMES);
        relatedPerson.Name = relatedNamesProperty.ToString();

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
            var labelProperty = properties.FindByName(Constants.Contact.RelatedPerson.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                switch (label)
                {
                    case Constants.Contact.RelatedPerson.CustomType.Father:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Father;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Mother:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Mother;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Parent:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Parent;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Brother:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Brother;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Sister:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Sister;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Child:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Child;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Friend:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Friend;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Spouse:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Spouse;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Partner:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Partner;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Assistant:
                        relatedPersonTypeFromInternal = RelatedPeopleType.Assistant;
                        break;
                    case Constants.Contact.RelatedPerson.CustomType.Manager:
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
    }

    /// <summary>Converts the related person to X-ABRELATEDNAMES property.</summary>
    public override CardPropertyList Serialize(RelatedPeople relatedPerson)
    {
        var properties = new List<CardProperty>();
        relatedPerson.Name = relatedPerson.Name.ThrowIfNull(nameof(relatedPerson.Name));
        var groupId = Guid.NewGuid().ToString();
        var urlProperty = new CardProperty(Constants.Contact.RelatedPerson.Property.X_ABRELATEDNAMES, relatedPerson.Name.ToString(), groupId);
        properties.Add(urlProperty);

        var relatedPersonTypeInternal = RelatedPeopleTypeMapping.GetType(relatedPerson.RelatedPersonType);
        if (relatedPerson.IsPreferred)
        {
            relatedPersonTypeInternal = relatedPersonTypeInternal.AddFlags(RelatedPeopleTypeInternal.Pref);
        }

        if (relatedPersonTypeInternal is not 0)
        {
            relatedPersonTypeInternal.StringArrayFlags()?.
                ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.RelatedPerson.Property.TYPE, type.ToUpper()));
        }

        var label = relatedPerson.RelatedPersonType switch
        {
            RelatedPeopleType.Father => Constants.Contact.RelatedPerson.CustomType.Father,
            RelatedPeopleType.Mother => Constants.Contact.RelatedPerson.CustomType.Mother,
            RelatedPeopleType.Parent => Constants.Contact.RelatedPerson.CustomType.Parent,
            RelatedPeopleType.Brother => Constants.Contact.RelatedPerson.CustomType.Brother,
            RelatedPeopleType.Sister => Constants.Contact.RelatedPerson.CustomType.Sister,
            RelatedPeopleType.Child => Constants.Contact.RelatedPerson.CustomType.Child,
            RelatedPeopleType.Friend => Constants.Contact.RelatedPerson.CustomType.Friend,
            RelatedPeopleType.Spouse => Constants.Contact.RelatedPerson.CustomType.Spouse,
            RelatedPeopleType.Partner => Constants.Contact.RelatedPerson.CustomType.Partner,
            RelatedPeopleType.Assistant => Constants.Contact.RelatedPerson.CustomType.Assistant,
            RelatedPeopleType.Manager => Constants.Contact.RelatedPerson.CustomType.Manager,
            RelatedPeopleType.Custom => relatedPerson.Label,
            _ => null,
        };

        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.RelatedPerson.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        return new(properties);
    }
}
