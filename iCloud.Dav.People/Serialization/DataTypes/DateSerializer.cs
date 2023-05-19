using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes
{
    /// <summary>
    /// Serializes and deserializes an <see cref="Date"/> object to and from a string representation, according to the vCard specification.
    /// </summary>
    public class DateSerializer : EncodableDataTypeSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateSerializer"/> class.
        /// </summary>
        public DateSerializer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateSerializer"/> class with the given <see cref="SerializationContext"/>.
        /// </summary>
        /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
        public DateSerializer(SerializationContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// Gets the Type that this <see cref="DateSerializer"/> can serialize and deserialize, which is <see cref="Date"/>.
        /// </summary>
        public override Type TargetType => typeof(Date);

        /// <summary>
        /// Converts a <see cref="Date"/> object to a string representation.
        /// </summary>
        /// <param name="obj">The <see cref="Date"/> object to be serialized.</param>
        /// <returns>A string representation of the <see cref="Date"/> object.</returns>
        public override string SerializeToString(object obj)
        {
            if (!(obj is Date date))
            {
                return null;
            }

            var value = date.DateTime.ToString();

            return Encode(date, value);
        }

        /// <summary>
        /// Converts a string representation of a <see cref="Date"/> object to a <see cref="Date"/> object.
        /// </summary>
        /// <param name="value">The string representation of the <see cref="Date"/> object to be deserialized.</param>
        /// <returns>A <see cref="Date"/> object.</returns>
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

        /// <summary>
        /// This method deserializes a <see cref="Date"/> object from the given <see cref="TextReader"/>.
        /// </summary>
        /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="Date"/> object from.</param>
        /// <returns>A <see cref="Date"/> object.</returns>
        public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
    }
}