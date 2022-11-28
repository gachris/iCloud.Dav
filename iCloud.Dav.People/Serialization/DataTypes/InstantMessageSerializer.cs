using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class InstantMessageSerializer : vCard.Net.Serialization.DataTypes.EncodableDataTypeSerializer
    {
        public InstantMessageSerializer() : base()
        {
        }

        public InstantMessageSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(InstantMessage);

        public override string SerializeToString(object obj)
        {
            if (!(obj is InstantMessage instantMessage))
            {
                return null;
            }

            var value = instantMessage.UserName;

            return Encode(instantMessage, value);
        }

        public InstantMessage Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is InstantMessage instantMessage))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(instantMessage, value);

            if (value is null)
            {
                return null;
            }

            instantMessage.UserName = value;

            return instantMessage;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}