using iCloud.Dav.People.Types;
using iCloud.Dav.People.Utils;

namespace iCloud.Dav.People.Serialization.Mapping;

internal class ProfileTypeMapping
{
    public static readonly TypeMapping<ProfileTypeInternal, ProfileType> Twitter = new(ProfileTypeInternal.Twitter, ProfileType.Twitter);
    public static readonly TypeMapping<ProfileTypeInternal, ProfileType> Facebook = new(ProfileTypeInternal.Facebook, ProfileType.Facebook);
    public static readonly TypeMapping<ProfileTypeInternal, ProfileType> LinkedIn = new(ProfileTypeInternal.LinkedIn, ProfileType.LinkedIn);
    public static readonly TypeMapping<ProfileTypeInternal, ProfileType> Flickr = new(ProfileTypeInternal.Flickr, ProfileType.Flickr);
    public static readonly TypeMapping<ProfileTypeInternal, ProfileType> Myspace = new(ProfileTypeInternal.Myspace, ProfileType.Myspace);
    public static readonly TypeMapping<ProfileTypeInternal, ProfileType> SinaWeibo = new(ProfileTypeInternal.SinaWeibo, ProfileType.SinaWeibo);

    private static readonly TypeMapping<ProfileTypeInternal, ProfileType>[] _typeMappings = new[]
    {
        Twitter,
        Facebook,
        LinkedIn,
        Flickr ,
        Myspace,
        SinaWeibo
    };

    public static ProfileType GetType(ProfileTypeInternal typeInternal)
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

    public static ProfileTypeInternal GetType(ProfileType type)
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
