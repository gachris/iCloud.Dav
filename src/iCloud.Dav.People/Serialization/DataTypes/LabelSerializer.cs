using System;
using System.IO;
using iCloud.Dav.People.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes an <see cref="Label"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class LabelSerializer : StringSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LabelSerializer"/> class.
    /// </summary>
    public LabelSerializer()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LabelSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public LabelSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the Type that this <see cref="LabelSerializer"/> can serialize and deserialize, which is <see cref="Label"/>.
    /// </summary>
    public override Type TargetType => typeof(Label);

    /// <summary>
    /// Converts a <see cref="Label"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="Label"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="Label"/> object.</returns>
    public override string SerializeToString(object obj)
    {
        return !(obj is Label label) ? null : Encode(label, label.Value);
    }

    /// <summary>
    /// Converts a string representation of a <see cref="Label"/> object to a <see cref="Label"/> object.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="Label"/> object to be deserialized.</param>
    /// <returns>A <see cref="Label"/> object.</returns>
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

    /// <summary>
    /// This method deserializes a <see cref="Label"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="Label"/> object from.</param>
    /// <returns>A <see cref="Label"/> object.</returns>
    public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}