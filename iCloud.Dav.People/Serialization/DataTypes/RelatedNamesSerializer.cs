using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class RelatedNamesSerializer : EncodableDataTypeSerializer
    {
        public RelatedNamesSerializer()
        {
        }

        public RelatedNamesSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(RelatedNames);

        public override string SerializeToString(object obj)
        {
            if (!(obj is RelatedNames relatedNames))
            {
                return null;
            }

            var value = relatedNames.Name;

            return Encode(relatedNames, value);
        }

        public RelatedNames Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is RelatedNames relatedNames))
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

            return relatedNames;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}