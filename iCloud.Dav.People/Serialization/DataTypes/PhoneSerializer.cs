using iCloud.Dav.People.DataTypes;
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

            return phone;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}