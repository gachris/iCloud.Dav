using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Utils;
using System.Linq;

namespace iCloud.vCard.Net.Serialization;

public class NameSerializer : SerializerBase<Name>
{
    public override void Deserialize(CardPropertyList properties, Name obj)
    {
        var property = properties.FindByName(Constants.Contact.N).ThrowIfNull(Constants.Contact.N);

        var values = property.ToString()?.Split(';');
        if (values is null) return;
        if (!values.Any()) return;

        obj.LastName = values[0];
        if (values.Length == 1)
            return;
        obj.FirstName = values[1];

        if (values.Length == 2)
            return;
        obj.MiddleName = values[2];

        if (values.Length == 3)
            return;
        obj.NamePrefix = values[3];

        if (values.Length == 4)
            return;
        obj.NameSuffix = values[4];
    }

    public override CardPropertyList Serialize(Name name)
    {
        var values = new ValueCollection(';')
        {
            name.LastName,
            name.FirstName,
            name.MiddleName,
            name.NamePrefix,
            name.NameSuffix
        };
        return new(new[] { new CardProperty(Constants.Contact.N, values) });
    }
}