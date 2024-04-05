using iCloud.Dav.People.Extensions;

namespace iCloud.Dav.People.DataTypes.Mapping;

internal class AddressTypeMapping
{
    public static readonly TypeMapping<AddressTypeInternal, AddressType> Home = new TypeMapping<AddressTypeInternal, AddressType>(AddressTypeInternal.Home, AddressType.Home);
    public static readonly TypeMapping<AddressTypeInternal, AddressType> Work = new TypeMapping<AddressTypeInternal, AddressType>(AddressTypeInternal.Work, AddressType.Work);
    public static readonly TypeMapping<AddressTypeInternal, AddressType> Other = new TypeMapping<AddressTypeInternal, AddressType>(AddressTypeInternal.Other, AddressType.Other);

    private static readonly TypeMapping<AddressTypeInternal, AddressType>[] _typeMappings = new[]
    {
        Home,
        Work,
        Other,
    };

    public static AddressType GetType(AddressTypeInternal typeInternal)
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

    public static AddressTypeInternal GetType(AddressType type)
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