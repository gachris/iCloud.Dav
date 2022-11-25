using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Serialization.DataTypes;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using vCard.Net.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.DataTypes;

public class X_ABRelatedNamesSerializer : EncodableDataTypeSerializer
{
    public X_ABRelatedNamesSerializer()
    {
    }

    public X_ABRelatedNamesSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    public override Type TargetType => typeof(X_ABRelatedNames);

    public override string? SerializeToString(object obj)
    {
        if (obj is not X_ABRelatedNames relatedNames)
        {
            return null;
        }

        // var properties = new List<CardProperty>();
        // relatedPerson.Name = relatedPerson.Name.ThrowIfNull(nameof(relatedPerson.Name));
        // var groupId = Guid.NewGuid().ToString();
        // var urlProperty = new CardProperty(Constants.Contact.RelatedPerson.Property.X_ABRELATEDNAMES, relatedPerson.Name.ToString(), groupId);
        // properties.Add(urlProperty);
           
        // var relatedPersonTypeInternal = RelatedPeopleTypeMapping.GetType(relatedPerson.RelatedPersonType);
        // if (relatedPerson.IsPreferred)
        // {
        //     relatedPersonTypeInternal = relatedPersonTypeInternal.AddFlags(RelatedPeopleTypeInternal.Pref);
        // }
           
        // if (relatedPersonTypeInternal is not 0)
        // {
        //     relatedPersonTypeInternal.StringArrayFlags()?.
        //         ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.RelatedPerson.Property.TYPE, type.ToUpper()));
        // }
           
        // var label = relatedPerson.RelatedPersonType switch
        // {
        //     RelatedPeopleType.Father => Constants.Contact.RelatedPerson.CustomType.Father,
        //     RelatedPeopleType.Mother => Constants.Contact.RelatedPerson.CustomType.Mother,
        //     RelatedPeopleType.Parent => Constants.Contact.RelatedPerson.CustomType.Parent,
        //     RelatedPeopleType.Brother => Constants.Contact.RelatedPerson.CustomType.Brother,
        //     RelatedPeopleType.Sister => Constants.Contact.RelatedPerson.CustomType.Sister,
        //     RelatedPeopleType.Child => Constants.Contact.RelatedPerson.CustomType.Child,
        //     RelatedPeopleType.Friend => Constants.Contact.RelatedPerson.CustomType.Friend,
        //     RelatedPeopleType.Spouse => Constants.Contact.RelatedPerson.CustomType.Spouse,
        //     RelatedPeopleType.Partner => Constants.Contact.RelatedPerson.CustomType.Partner,
        //     RelatedPeopleType.Assistant => Constants.Contact.RelatedPerson.CustomType.Assistant,
        //     RelatedPeopleType.Manager => Constants.Contact.RelatedPerson.CustomType.Manager,
        //     RelatedPeopleType.Custom => relatedPerson.Label,
        //     _ => null,
        // };
           
        // if (label is not null)
        // {
        //     var labelProperty = new CardProperty(Constants.Contact.RelatedPerson.Property.X_ABLABEL, label, groupId);
        //     properties.Add(labelProperty);
        // }
           
        // return new(properties);

        var value = relatedNames.Name;
        return Encode(relatedNames, value);
    }

    public X_ABRelatedNames? Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (CreateAndAssociate() is not X_ABRelatedNames relatedNames)
        {
            return null;
        }

        // Decode the value, if necessary!
        value = Decode(relatedNames, value);

        if (value is null)
        {
            return null;
        }

        relatedNames.Name = value;

        var types = relatedNames.Parameters.GetMany("TYPE");

        _ = types.TryParse<RelatedPeopleTypeInternal>(out var relatedPersonTypeInternal);
        var isPreferred = relatedPersonTypeInternal.HasFlag(RelatedPeopleTypeInternal.Pref);
        if (isPreferred)
        {
            relatedNames.IsPreferred = true;
            relatedPersonTypeInternal = relatedPersonTypeInternal.RemoveFlags(RelatedPeopleTypeInternal.Pref);
        }

        var relatedPersonTypeFromInternal = RelatedPeopleTypeMapping.GetType(relatedPersonTypeInternal);
        if (relatedPersonTypeFromInternal is 0)
        {
            //var labelProperty = properties.FindByName(Constants.Contact.RelatedPerson.Property.X_ABLABEL);
            //if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            //{
            //    switch (label)
            //    {
            //        case Constants.Contact.RelatedPerson.CustomType.Father:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Father;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Mother:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Mother;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Parent:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Parent;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Brother:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Brother;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Sister:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Sister;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Child:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Child;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Friend:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Friend;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Spouse:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Spouse;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Partner:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Partner;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Assistant:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Assistant;
            //            break;
            //        case Constants.Contact.RelatedPerson.CustomType.Manager:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Manager;
            //            break;
            //        default:
            //            relatedPersonTypeFromInternal = RelatedPeopleType.Custom;
            //            relatedPerson.Label = label;
            //            break;
            //    }
            //}
        }

        relatedNames.RelatedPersonType = relatedPersonTypeFromInternal;

        return relatedNames;
    }

    public override object? Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}
