using iCloud.Dav.People.Extensions;

namespace iCloud.Dav.People.DataTypes.Mapping;

internal class RelatedNamesMapping
{
    public static readonly TypeMapping<RelatedNamesTypeInternal, RelatedNamesType> Other = new TypeMapping<RelatedNamesTypeInternal, RelatedNamesType>(RelatedNamesTypeInternal.Other, RelatedNamesType.Other);

    private static readonly TypeMapping<RelatedNamesTypeInternal, RelatedNamesType>[] _typeMappings = new[]
    {
        Other,
    };

    public static RelatedNamesType GetType(RelatedNamesTypeInternal typeInternal)
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

    public static RelatedNamesTypeInternal GetType(RelatedNamesType type)
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