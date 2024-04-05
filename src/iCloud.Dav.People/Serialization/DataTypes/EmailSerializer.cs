using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes an <see cref="Email"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class EmailSerializer : EncodableDataTypeSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSerializer"/> class.
    /// </summary>
    public EmailSerializer()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public EmailSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the Type that this <see cref="EmailSerializer"/> can serialize and deserialize, which is <see cref="Email"/>.
    /// </summary>
    public override Type TargetType => typeof(Email);

    /// <summary>
    /// Converts an <see cref="Email"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="Email"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="Email"/> object.</returns>
    public override string SerializeToString(object obj)
    {
        if (!(obj is Email email))
        {
            return null;
        }

        var value = email.Address;

        return Encode(email, value);
    }

    /// <summary>
    /// Converts a string representation of an <see cref="Email"/> object to an <see cref="Email"/> object.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="Email"/> object to be deserialized.</param>
    /// <returns>An <see cref="Email"/> object.</returns>
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

    /// <summary>
    /// This method deserializes an <see cref="Email"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="Email"/> object from.</param>
    /// <returns>An <see cref="Email"/> object.</returns>
    public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}