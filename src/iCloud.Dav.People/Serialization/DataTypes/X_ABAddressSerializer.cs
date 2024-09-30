using iCloud.Dav.People.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes a <see cref="X_ABAddressSerializer"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class X_ABAddressSerializer : StringSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="X_ABAddressSerializer"/> class.
    /// </summary>
    public X_ABAddressSerializer()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="X_ABAddressSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public X_ABAddressSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the Type that this <see cref="X_ABAddressSerializer"/> can serialize and deserialize, which is <see cref="X_ABAddress"/>.
    /// </summary>
    public override Type TargetType => typeof(X_ABAddress);

    /// <summary>
    /// Converts a <see cref="X_ABAddress"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="X_ABAddress"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="X_ABAddress"/> object.</returns>
    public override string SerializeToString(object obj)
    {
        return obj is not X_ABAddress aBAddress ? null : Encode(aBAddress, aBAddress.Value);
    }

    /// <summary>
    /// Converts a string representation of a <see cref="X_ABAddress"/> object to a <see cref="X_ABAddress"/> object.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="X_ABAddress"/> object to be deserialized.</param>
    /// <returns>A <see cref="X_ABAddress"/> object.</returns>
    public X_ABAddress Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (CreateAndAssociate() is not X_ABAddress aBAddress)
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

    /// <summary>
    /// This method deserializes a <see cref="X_ABAddress"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="X_ABAddress"/> object from.</param>
    /// <returns>A <see cref="X_ABAddress"/> object.</returns>
    public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}