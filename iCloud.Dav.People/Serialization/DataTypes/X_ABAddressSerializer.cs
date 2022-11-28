using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class X_ABAddressSerializer : StringSerializer
    {
        public X_ABAddressSerializer()
        {
        }

        public X_ABAddressSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(X_ABAddress);

        public override string SerializeToString(object obj)
        {
            return !(obj is X_ABAddress aBAddress) ? null : Encode(aBAddress, aBAddress.Value);
        }

        public X_ABAddress Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is X_ABAddress aBAddress))
            {
                return null;
            }

            // Decode the value as needed
            value = Decode(aBAddress, value);

            if (value is null)
            {
                return null;
            }

            aBAddress.Value = value;

            return aBAddress;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}