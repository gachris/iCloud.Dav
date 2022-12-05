using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    public class DateSerializer : EncodableDataTypeSerializer
    {
        public DateSerializer()
        {
        }

        public DateSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        public override Type TargetType => typeof(Date);

        public override string SerializeToString(object obj)
        {
            if (!(obj is Date date))
            {
                return null;
            }

            var value = date.DateTime.ToString();

            return Encode(date, value);
        }

        public Date Deserialize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (!(CreateAndAssociate() is Date date))
            {
                return null;
            }

            // Decode the value, if necessary!
            value = Decode(date, value);

            if (value is null)
            {
                return null;
            }

            date.DateTime = DateTimeHelper.TryParseDate(value) ?? DateTime.MinValue;

            return date;
        }

        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}