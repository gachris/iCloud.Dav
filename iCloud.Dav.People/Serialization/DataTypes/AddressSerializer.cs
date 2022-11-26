using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class AddressSerializer : StringSerializer
    {
        public AddressSerializer()
        {
        }

        public AddressSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(Address);

        public override string SerializeToString(object obj)
        {
            if (!(obj is Address address))
            {
                return null;
            }

            //    var sc = obj as StatusCode;
            //    if (sc == null)
            //    {
            //        return null;
            //    }

            //    var vals = new string[sc.Parts.Length];
            //    for (var i = 0; i < sc.Parts.Length; i++)
            //    {
            //        vals[i] = sc.Parts[i].ToString();
            //    }
            //    return Encode(sc, Escape(string.Join(".", vals)));

            //    var properties = new List<CardProperty>();
            //    if (IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address), "Cannot be empty!");
            //    var valueCollection = new ValueCollection(';')
            //    {
            //        string.Empty,
            //        string.Empty,
            //        address.Street,
            //        address.City,
            //        address.Region,
            //        address.PostalCode,
            //        address.Country
            //    };
            //    var groupId = Guid.NewGuid().ToString();
            //    var addressProperty = new CardProperty(Constants.Contact.Address.Property.ADR, valueCollection)
            //    {
            //        Group = groupId
            //    };
            //    properties.Add(addressProperty);

            //    var addressTypeInternal = AddressTypeMapping.GetType(address.AddressType);
            //    if (address.IsPreferred)
            //    {
            //        addressTypeInternal = addressTypeInternal.AddFlags(AddressTypeInternal.Pref);
            //    }

            //    if (addressTypeInternal is not 0)
            //    {
            //        addressTypeInternal.StringArrayFlags()?.
            //            ForEach(type => addressProperty.Subproperties.Add(Constants.Contact.Address.Property.TYPE, type.ToUpper()));
            //    }

            //    var label = address.AddressType switch
            //    {
            //        AddressType.School => Constants.Contact.Address.CustomType.School,
            //        AddressType.Custom => address.Label,
            //        _ => null,
            //    };

            //    if (label is not null)
            //    {
            //        var labelProperty = new CardProperty(Constants.Contact.Address.Property.X_ABLABEL, label, groupId);
            //        properties.Add(labelProperty);
            //    }

            //    if (!string.IsNullOrEmpty(address.CountryCode))
            //    {
            //        var abAdrProperty = new CardProperty(Constants.Contact.Address.Property.X_ABADR, address.CountryCode, groupId);
            //        properties.Add(abAdrProperty);
            //    }

            //    return new(properties);

            var value = address.Label;
            return Encode(address, value);
        }

        public Address Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is Address address))
            {
                return null;
            }

            // Decode the value as needed
            value = Decode(address, value);

            if (value is null)
            {
                return null;
            }

            var parts = value.Split(';');

            if (parts.Length >= 7)
                address.Country = parts[6].Trim();
            if (parts.Length >= 6)
                address.PostalCode = parts[5].Trim();
            if (parts.Length >= 5)
                address.Region = parts[4].Trim();
            if (parts.Length >= 4)
                address.City = parts[3].Trim();
            if (parts.Length >= 3)
                address.Street = parts[2].Trim();

            // address.CountryCode = properties.FindByName(Constants.Contact.Address.Property.X_ABADR)?.Value?.ToString();

            var types = address.Parameters.GetMany("TYPE");

            _ = types.TryParse<AddressTypeInternal>(out var addressTypeInternal);
            var isPreferred = addressTypeInternal.HasFlag(AddressTypeInternal.Pref);
            if (isPreferred)
            {
                address.IsPreferred = true;
                addressTypeInternal = addressTypeInternal.RemoveFlags(AddressTypeInternal.Pref);
            }

            var addressTypeFromInternal = AddressTypeMapping.GetType(addressTypeInternal);
            if (addressTypeFromInternal is 0)
            {
                //var labelProperty = properties.FindByName(Constants.Contact.Address.Property.X_ABLABEL);
                //if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
                //{
                //    switch (label)
                //    {
                //        case Constants.Contact.Address.CustomType.School:
                //            addressTypeFromInternal = AddressType.School;
                //            break;
                //        default:
                //            addressTypeFromInternal = AddressType.Custom;
                //            address.Label = label;
                //            break;
                //    }
                //}
            }

            address.AddressType = addressTypeFromInternal;

            return address;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());

        private static bool IsNullOrEmpty(Address address)
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
}