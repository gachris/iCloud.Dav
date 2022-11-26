using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class PhoneSerializer : EncodableDataTypeSerializer
    {
        public PhoneSerializer()
        {
        }

        public PhoneSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(Phone);

        public override string SerializeToString(object obj)
        {
            if (!(obj is Phone phone))
            {
                return null;
            }
            //    var properties = new List<CardProperty>();
            //    phone.FullNumber = phone.FullNumber.ThrowIfNull(nameof(phone.FullNumber));
            //    var groupId = Guid.NewGuid().ToString();
            //    var telProperty = new CardProperty(Constants.Contact.Phone.Property.TEL, phone.FullNumber, groupId);
            //    properties.Add(telProperty);

            //    var phoneTypeInternal = PhoneTypeMapping.GetType(phone.PhoneType);
            //    if (phone.IsPreferred)
            //    {
            //        phoneTypeInternal = phoneTypeInternal.AddFlags(PhoneTypeInternal.Pref);
            //    }

            //    if (phoneTypeInternal is not 0)
            //    {
            //        phoneTypeInternal.StringArrayFlags()?.
            //            ForEach(type => telProperty.Subproperties.Add(Constants.Contact.Phone.Property.TYPE, type.ToUpper()));
            //    }

            //    var label = phone.PhoneType switch
            //    {
            //        PhoneType.AppleWatch => Constants.Contact.Phone.CustomType.AppleWatch,
            //        PhoneType.School => Constants.Contact.Phone.CustomType.School,
            //        PhoneType.Custom => phone.Label,
            //        _ => null,
            //    };

            //    if (label is not null)
            //    {
            //        var labelProperty = new CardProperty(Constants.Contact.Phone.Property.X_ABLABEL, label, groupId);
            //        properties.Add(labelProperty);
            //    }

            //    return new(properties);
            var value = phone.FullNumber;
            return Encode(phone, value);
        }

        public Phone Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is Phone phone))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(phone, value);

            if (value is null)
            {
                return null;
            }

            phone.FullNumber = value;

            var types = phone.Parameters.GetMany("TYPE");

            _ = types.TryParse<PhoneTypeInternal>(out var phoneTypeInternal);
            var isPreferred = phoneTypeInternal.HasFlag(PhoneTypeInternal.Pref);
            if (isPreferred)
            {
                phone.IsPreferred = true;
                phoneTypeInternal = phoneTypeInternal.RemoveFlags(PhoneTypeInternal.Pref);
            }

            var phoneTypeFromInternal = PhoneTypeMapping.GetType(phoneTypeInternal);
            if (phoneTypeFromInternal is 0)
            {
                //var labelProperty = properties.FindByName(Constants.Contact.Phone.Property.X_ABLABEL);
                //if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
                //{
                //    switch (label)
                //    {
                //        case Constants.Contact.Phone.CustomType.AppleWatch:
                //            phoneTypeFromInternal = PhoneType.AppleWatch;
                //            break;
                //        case Constants.Contact.Phone.CustomType.School:
                //            phoneTypeFromInternal = PhoneType.School;
                //            break;
                //        default:
                //            phoneTypeFromInternal = PhoneType.Custom;
                //            phone.Label = label;
                //            break;
                //    }
                //}
            }

            phone.PhoneType = phoneTypeFromInternal;

            return phone;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}