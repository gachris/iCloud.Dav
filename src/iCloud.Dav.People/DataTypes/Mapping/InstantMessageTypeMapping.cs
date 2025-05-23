﻿using iCloud.Dav.People.Extensions;

namespace iCloud.Dav.People.DataTypes.Mapping;

internal class InstantMessageTypeMapping
{
    public static readonly TypeMapping<InstantMessageTypeInternal, InstantMessageType> Home = new TypeMapping<InstantMessageTypeInternal, InstantMessageType>(InstantMessageTypeInternal.Home, InstantMessageType.Home);
    public static readonly TypeMapping<InstantMessageTypeInternal, InstantMessageType> Work = new TypeMapping<InstantMessageTypeInternal, InstantMessageType>(InstantMessageTypeInternal.Work, InstantMessageType.Work);
    public static readonly TypeMapping<InstantMessageTypeInternal, InstantMessageType> Facebook = new TypeMapping<InstantMessageTypeInternal, InstantMessageType>(InstantMessageTypeInternal.Facebook, InstantMessageType.Facebook);
    public static readonly TypeMapping<InstantMessageTypeInternal, InstantMessageType> Other = new TypeMapping<InstantMessageTypeInternal, InstantMessageType>(InstantMessageTypeInternal.Other, InstantMessageType.Other);

    private static readonly TypeMapping<InstantMessageTypeInternal, InstantMessageType>[] _typeMappings =
    [
        Home,
        Work,        
        Facebook,
        Other,
    ];

    public static InstantMessageType GetType(InstantMessageTypeInternal typeInternal)
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

    public static InstantMessageTypeInternal GetType(InstantMessageType type)
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