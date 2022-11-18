using iCloud.vCard.Net.Serialization;

namespace iCloud.vCard.Net.Data;

public class Organization : CardDataType
{
    /// <summary>
    /// The description the or company of the organization.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The department of the organization.
    /// </summary>
    public string? Department { get; set; }

    public Organization()
    {
    }

    public Organization(CardPropertyList properties)
    {
        var orgSerializer = new OrganizationSerializer();
        orgSerializer.Deserialize(properties, this);
    }
}
