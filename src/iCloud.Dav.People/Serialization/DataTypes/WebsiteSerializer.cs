using iCloud.Dav.People.DataTypes;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes a <see cref="Website"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class WebsiteSerializer : EncodableDataTypeSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsiteSerializer"/> class.
    /// </summary>
    public WebsiteSerializer() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsiteSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public WebsiteSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the Type that this <see cref="WebsiteSerializer"/> can serialize and deserialize, which is <see cref="Website"/>.
    /// </summary>
    public override Type TargetType => typeof(Website);

    /// <summary>
    /// Converts a <see cref="Website"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="Website"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="Website"/> object.</returns>
    public override string SerializeToString(object obj)
    {
        if (!(obj is Website website))
        {
            return null;
        }

        var value = website.Url;

        return Encode(website, value);
    }

    /// <summary>
    /// Converts a string representation of a <see cref="Website"/> object to a <see cref="Website"/> object.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="Website"/> object to be deserialized.</param>
    /// <returns>A <see cref="Website"/> object.</returns>
    public Website Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!(CreateAndAssociate() is Website website))
        {
            return null;
        }

        // Decode the value, if necessary!
        value = Decode(website, value);

        if (value is null)
        {
            return null;
        }

        website.Url = value;

        return website;
    }


    /// <summary>
    /// This method deserializes a <see cref="Website"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="Website"/> object from.</param>
    /// <returns>A <see cref="Website"/> object.</returns>
    public override object Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}