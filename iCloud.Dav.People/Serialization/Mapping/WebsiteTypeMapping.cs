using iCloud.Dav.People.Types;
using iCloud.Dav.People.Utils;

namespace iCloud.Dav.People.Serialization.Mapping;

internal class WebsiteTypeMapping
{
    public static readonly TypeMapping<WebsiteTypeInternal, WebsiteType> Home = new(WebsiteTypeInternal.Home, WebsiteType.Home);
    public static readonly TypeMapping<WebsiteTypeInternal, WebsiteType> Work = new(WebsiteTypeInternal.Work, WebsiteType.Work);
    public static readonly TypeMapping<WebsiteTypeInternal, WebsiteType> Other = new(WebsiteTypeInternal.Other, WebsiteType.Other);

    private static readonly TypeMapping<WebsiteTypeInternal, WebsiteType>[] _typeMappings = new[]
    {
        Home,
        Work,
        Other,
    };

    public static WebsiteType GetType(WebsiteTypeInternal typeInternal)
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

    public static WebsiteTypeInternal GetType(WebsiteType type)
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