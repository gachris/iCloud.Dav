using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Utils;
using System.Linq;

namespace iCloud.vCard.Net.Serialization;

public class OrganizationSerializer : SerializerBase<Organization>
{
    public override void Deserialize(CardPropertyList properties, Organization obj)
    {
        var property = properties.FindByName(Constants.Contact.ORG).ThrowIfNull(Constants.Contact.N);

        var values = property.ToString()?.Split(';');
        if (values is null) return;
        if (!values.Any()) return;

        obj.Description = values[0];
        if (values.Length == 1)
            return;
        obj.Department = values[1];
    }

    public override CardPropertyList Serialize(Organization org)
    {
        var values = new ValueCollection(';')
        {
            org.Description,
            org.Department
        };
        return new(new[] { new CardProperty(Constants.Contact.ORG, values) });
    }
}
