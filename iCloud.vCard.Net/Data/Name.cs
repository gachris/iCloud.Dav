using iCloud.vCard.Net.Serialization;

namespace iCloud.vCard.Net.Data;

public class Name : CardDataType
{
    /// <summary>
    /// The last name of the contact.
    /// </summary>
    public virtual string? LastName { get; set; }

    /// <summary>
    /// The given (first) name of the contact.
    /// </summary>
    public virtual string? FirstName { get; set; }

    /// <summary>
    /// Any additional (e.g. middle) names of the contact.
    /// </summary>
    public virtual string? MiddleName { get; set; }

    /// <summary>
    /// The prefix (e.g. "Mr.") of the contact.
    /// </summary>
    public virtual string? NamePrefix { get; set; }

    /// <summary>
    /// The suffix (e.g. "Jr.") of the contact.
    /// </summary>
    public virtual string? NameSuffix { get; set; }

    public Name()
    {
    }

    public Name(CardPropertyList properties)
    {
        var nameSerializer = new NameSerializer();
        nameSerializer.Deserialize(properties, this);
    }
}
