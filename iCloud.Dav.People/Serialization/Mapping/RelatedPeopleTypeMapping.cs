using iCloud.Dav.People.Types;
using iCloud.Dav.People.Utils;

namespace iCloud.Dav.People.Serialization.Mapping;

internal class RelatedPeopleTypeMapping
{
    public static readonly TypeMapping<RelatedPeopleTypeInternal, RelatedPeopleType> Other = new(RelatedPeopleTypeInternal.Other , RelatedPeopleType.Other);

    private static readonly TypeMapping<RelatedPeopleTypeInternal, RelatedPeopleType>[] _typeMappings = new[]
    {
        Other,
    };

    public static RelatedPeopleType GetType(RelatedPeopleTypeInternal typeInternal)
    {
        foreach (var typeMapping in _typeMappings)
        {
            if (typeInternal.HasFlags(typeMapping.TypeInternal))
            {
                return typeMapping.Type;
            }
        }

        return 0;
    }

    public static RelatedPeopleTypeInternal GetType(RelatedPeopleType type)
    {
        foreach (var typeMapping in _typeMappings)
        {
            if (typeMapping.Type == type)
            {
                return typeMapping.TypeInternal;
            }
        }

        return 0;
    }
}