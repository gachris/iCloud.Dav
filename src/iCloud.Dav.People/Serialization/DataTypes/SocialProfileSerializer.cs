using System;
using System.IO;
using iCloud.Dav.People.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes a <see cref="SocialProfile"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class SocialProfileSerializer : EncodableDataTypeSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SocialProfileSerializer"/> class.
    /// </summary>
    public SocialProfileSerializer() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SocialProfileSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public SocialProfileSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the Type that this <see cref="SocialProfileSerializer"/> can serialize and deserialize, which is <see cref="SocialProfile"/>.
    /// </summary>
    public override Type TargetType => typeof(SocialProfile);

    /// <summary>
    /// Converts a <see cref="SocialProfile"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="SocialProfile"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="SocialProfile"/> object.</returns>
    public override string SerializeToString(object obj)
    {
        if (!(obj is SocialProfile socialProfile))
        {
            return null;
        }

        var value = socialProfile.Url;

        return Encode(socialProfile, value);
    }

    /// <summary>
    /// Converts a string representation of a <see cref="SocialProfile"/> object to a <see cref="SocialProfile"/> object.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="SocialProfile"/> object to be deserialized.</param>
    /// <returns>A <see cref="SocialProfile"/> object.</returns>
    public SocialProfile Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!(CreateAndAssociate() is SocialProfile socialProfile))
        {
            return null;
        }

        // Decode the value, if necessary!
        value = Decode(socialProfile, value);

        if (value is null)
        {
            return null;
        }

        socialProfile.Url = value;

        return socialProfile;
    }

    /// <summary>
    /// This method deserializes a <see cref="SocialProfile"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="SocialProfile"/> object from.</param>
    /// <returns>A <see cref="SocialProfile"/> object.</returns>
    public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}