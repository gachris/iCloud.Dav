using iCloud.Dav.People.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes an <see cref="Phone"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class PhoneSerializer : EncodableDataTypeSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PhoneSerializer"/> class.
    /// </summary>
    public PhoneSerializer()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PhoneSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public PhoneSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the Type that this <see cref="PhoneSerializer"/> can serialize and deserialize, which is <see cref="Phone"/>.
    /// </summary>
    public override Type TargetType => typeof(Phone);

    /// <summary>
    /// Converts a <see cref="Phone"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="Phone"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="Phone"/> object.</returns>
    public override string SerializeToString(object obj)
    {
        if (obj is not Phone phone)
        {
            return null;
        }

        var value = phone.FullNumber;

        return Encode(phone, value);
    }

    /// <summary>
    /// Converts a string representation of a <see cref="Phone"/> object to a <see cref="Phone"/> object.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="Phone"/> object to be deserialized.</param>
    /// <returns>A <see cref="Phone"/> object.</returns>
    public Phone Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (CreateAndAssociate() is not Phone phone)
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

    /// <summary>
    /// This method deserializes a <see cref="Phone"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="Phone"/> object from.</param>
    /// <returns>A <see cref="Phone"/> object.</returns>
    public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}