using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class EmailSerializer : EncodableDataTypeSerializer
    {
        public EmailSerializer()
        {
        }

        public EmailSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(Email);

        public override string SerializeToString(object obj)
        {
            if (!(obj is Email email))
            {
                return null;
            }

            var value = email.Address;

            return Encode(email, value);
        }

        public Email Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is Email email))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(email, value);

            if (value is null)
            {
                return null;
            }

            email.Address = value;

            return email;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}