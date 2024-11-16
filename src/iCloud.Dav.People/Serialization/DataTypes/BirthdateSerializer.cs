using iCloud.Dav.People.DataTypes;
using vCard.Net.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes a <see cref="Birthdate"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class BirthdateSerializer : DataTypeSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BirthdateSerializer"/> class.
    /// </summary>
    public BirthdateSerializer()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BirthdateSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public BirthdateSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the type that this serializer can serialize and deserialize, which is <see cref="Birthdate"/>.
    /// </summary>
    public override Type TargetType => typeof(Birthdate);

    /// <summary>
    /// Converts a <see cref="Birthdate"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="Birthdate"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="Birthdate"/> object, or <see langword="null"/> if the object is not of type <see cref="Birthdate"/>.</returns>
    public override string SerializeToString(object obj)
    {
        if (obj is not Birthdate birthdate || birthdate.Date == null)
        {
            return null;
        }

        return birthdate.Date.Value.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Converts a string representation of a birthdate to a <see cref="Birthdate"/> object.
    /// </summary>
    /// <param name="value">The string representation of the birthdate (e.g., "2023-11-16").</param>
    /// <returns>A <see cref="Birthdate"/> object, or <see langword="null"/> if the input is invalid.</returns>
    public Birthdate Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (CreateAndAssociate() is not Birthdate birthdate)
        {
            return null;
        }

        if (DateTime.TryParse(value, out var date))
        {
            birthdate.Date = new VCardDateTime(date)
            {
                HasTime = false
            };

            return birthdate;
        }

        return null;
    }

    /// <summary>
    /// Deserializes a <see cref="Birthdate"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="Birthdate"/> object from.</param>
    /// <returns>A <see cref="Birthdate"/> object, or <see langword="null"/> if deserialization fails.</returns>
    public override object Deserialize(TextReader tr)
    {
        if (tr == null)
        {
            return null;
        }

        // Deserialize the text content into a Birthdate object.
        return Deserialize(tr.ReadToEnd());
    }
}
