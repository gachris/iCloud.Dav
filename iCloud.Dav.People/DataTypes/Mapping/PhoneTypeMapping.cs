using iCloud.Dav.People.Utils;

namespace iCloud.Dav.People.DataTypes.Mapping;

internal class PhoneTypeMapping
{
    public static readonly TypeMapping<PhoneTypeInternal, PhoneType> iPhone = new(PhoneTypeInternal.iPhone | PhoneTypeInternal.Cell | PhoneTypeInternal.Voice, PhoneType.iPhone);
    public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Home = new(PhoneTypeInternal.Home | PhoneTypeInternal.Voice, PhoneType.Home);
    public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Work = new(PhoneTypeInternal.Work | PhoneTypeInternal.Voice, PhoneType.Work);
    public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Main = new(PhoneTypeInternal.Main, PhoneType.Main);
    public static readonly TypeMapping<PhoneTypeInternal, PhoneType> HomeFax = new(PhoneTypeInternal.Fax | PhoneTypeInternal.Home, PhoneType.HomeFax);
    public static readonly TypeMapping<PhoneTypeInternal, PhoneType> WorkFax = new(PhoneTypeInternal.Fax | PhoneTypeInternal.Work, PhoneType.WorkFax);
    public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Pager = new(PhoneTypeInternal.Pager, PhoneType.Pager);
    public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Other = new(PhoneTypeInternal.Voice | PhoneTypeInternal.Other, PhoneType.Other);
    public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Mobile = new(PhoneTypeInternal.Cell | PhoneTypeInternal.Voice, PhoneType.Mobile);

    private static readonly TypeMapping<PhoneTypeInternal, PhoneType>[] _typeMappings = new[]
    {
        iPhone,
        Home,
        Work,
        Main,
        HomeFax,
        WorkFax,
        Pager,
        Other,
        Mobile
    };

    public static PhoneType GetType(PhoneTypeInternal typeInternal)
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

    public static PhoneTypeInternal GetType(PhoneType type)
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