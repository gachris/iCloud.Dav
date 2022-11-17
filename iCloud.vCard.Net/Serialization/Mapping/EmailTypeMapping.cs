using iCloud.vCard.Net.Types;
using iCloud.vCard.Net.Utils;

namespace iCloud.vCard.Net.Serialization.Mapping;

internal class EmailTypeMapping
{
    public static readonly TypeMapping<EmailTypeInternal, EmailType> Home = new(EmailTypeInternal.Home | EmailTypeInternal.Internet, EmailType.Home);
    public static readonly TypeMapping<EmailTypeInternal, EmailType> Work = new(EmailTypeInternal.Work | EmailTypeInternal.Internet, EmailType.Work);
    public static readonly TypeMapping<EmailTypeInternal, EmailType> Other = new(EmailTypeInternal.Other | EmailTypeInternal.Internet, EmailType.Other);

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