﻿using iCloud.Dav.People.Extensions;

namespace iCloud.Dav.People.DataTypes.Mapping;

internal class EmailTypeMapping
{
    public static readonly TypeMapping<EmailTypeInternal, EmailType> Home = new TypeMapping<EmailTypeInternal, EmailType>(EmailTypeInternal.Home | EmailTypeInternal.Internet, EmailType.Home);
    public static readonly TypeMapping<EmailTypeInternal, EmailType> Work = new TypeMapping<EmailTypeInternal, EmailType>(EmailTypeInternal.Work | EmailTypeInternal.Internet, EmailType.Work);
    public static readonly TypeMapping<EmailTypeInternal, EmailType> Other = new TypeMapping<EmailTypeInternal, EmailType>(EmailTypeInternal.Other | EmailTypeInternal.Internet, EmailType.Other);

    private static readonly TypeMapping<EmailTypeInternal, EmailType>[] _typeMappings = new[]
    {
        Home,
        Work,
        Other,
    };

    public static EmailType GetType(EmailTypeInternal typeInternal)
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

    public static EmailTypeInternal GetType(EmailType type)
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