using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes a <see cref="RelatedNames"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class RelatedNamesSerializer : EncodableDataTypeSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RelatedNamesSerializer"/> class.
    /// </summary>
    public RelatedNamesSerializer()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelatedNamesSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public RelatedNamesSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the Type that this <see cref="RelatedNamesSerializer"/> can serialize and deserialize, which is <see cref="RelatedNames"/>.
    /// </summary>
    public override Type TargetType => typeof(RelatedNames);

    /// <summary>
    /// Converts a <see cref="RelatedNames"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="RelatedNames"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="RelatedNames"/> object.</returns>
    public override string SerializeToString(object obj)
    {
        if (!(obj is RelatedNames relatedNames))
        {
            return null;
        }

        var value = relatedNames.Name;

        return Encode(relatedNames, value);
    }

    /// <summary>
    /// Converts a string representation of a <see cref="RelatedNames"/> object to a <see cref="RelatedNames"/> object.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="RelatedNames"/> object to be deserialized.</param>
    /// <returns>A <see cref="RelatedNames"/> object.</returns>
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

    /// <summary>
    /// Deserializes a <see cref="RelatedNames"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="RelatedNames"/> object from.</param>
    /// <returns>A <see cref="RelatedNames"/> object.</returns>
    public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}