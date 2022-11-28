using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class LabelSerializer : StringSerializer
    {
        public LabelSerializer()
        {
        }

        public LabelSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(Label);

        public override string SerializeToString(object obj)
        {
            return !(obj is Label label) ? null : Encode(label, label.Value);
        }

        public Label Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is Label label))
            {
                return null;
            }

            // Decode the value as needed
            value = Decode(label, value);

            if (value is null)
            {
                return null;
            }

            label.Value = value;

            return label;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}