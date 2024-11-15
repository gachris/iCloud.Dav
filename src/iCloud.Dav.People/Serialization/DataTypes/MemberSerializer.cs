using iCloud.Dav.People.DataTypes;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

/// <summary>
/// Serializes and deserializes a <see cref="Member"/> object to and from a string representation, according to the vCard specification.
/// </summary>
public class MemberSerializer : DataTypeSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberSerializer"/> class.
    /// </summary>
    public MemberSerializer()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemberSerializer"/> class with the given <see cref="SerializationContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="SerializationContext"/> to use.</param>
    public MemberSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    /// <summary>
    /// Gets the type that this <see cref="MemberSerializer"/> can serialize and deserialize, which is <see cref="Member"/>.
    /// </summary>
    public override Type TargetType => typeof(Member);

    /// <summary>
    /// Converts a <see cref="Member"/> object to a string representation.
    /// </summary>
    /// <param name="obj">The <see cref="Member"/> object to be serialized.</param>
    /// <returns>A string representation of the <see cref="Member"/> object, or <see langword="null"/> if the object is not of type <see cref="Member"/>.</returns>
    public override string SerializeToString(object obj)
    {
        if (obj is not Member member)
        {
            return null;
        }

        if (string.IsNullOrEmpty(member.Uid))
        {
            return null;
        }

        // Serialize the Member object to a string in the format "urn:uuid:{Uid}".
        return $"urn:uuid:{member.Uid}";
    }

    /// <summary>
    /// Converts a string representation of a <see cref="Member"/> object to a <see cref="Member"/> object.
    /// </summary>
    /// <param name="value">The string representation of the <see cref="Member"/> object to be deserialized.</param>
    /// <returns>A <see cref="Member"/> object, or <see langword="null"/> if the input is invalid.</returns>
    public Member Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        // Create a new Member instance using the base deserialization context.
        if (CreateAndAssociate() is not Member member)
        {
            return null;
        }

        if (value is null)
        {
            return null;
        }

        // Remove the "urn:uuid:" prefix if it exists in the input string.
        value = value.Replace("urn:uuid:", string.Empty);

        // Set the Uid of the Member object.
        member.Uid = value;

        return member;
    }

    /// <summary>
    /// Deserializes a <see cref="Member"/> object from the given <see cref="TextReader"/>.
    /// </summary>
    /// <param name="tr">The <see cref="TextReader"/> to deserialize the <see cref="Member"/> object from.</param>
    /// <returns>A <see cref="Member"/> object, or <see langword="null"/> if deserialization fails.</returns>
    public override object Deserialize(TextReader tr)
    {
        if (tr == null)
        {
            return null;
        }

        // Deserialize the text content into a Member object.
        return Deserialize(tr.ReadToEnd());
    }
}
