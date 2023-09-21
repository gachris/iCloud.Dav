using iCloud.Dav.People.Extensions;

namespace iCloud.Dav.People.DataTypes.Mapping
{
    internal class SocialProfileTypeMapping
    {
        public static readonly TypeMapping<SocialProfileTypeInternal, SocialProfileType> Twitter = new TypeMapping<SocialProfileTypeInternal, SocialProfileType>(SocialProfileTypeInternal.Twitter, SocialProfileType.Twitter);
        public static readonly TypeMapping<SocialProfileTypeInternal, SocialProfileType> Facebook = new TypeMapping<SocialProfileTypeInternal, SocialProfileType>(SocialProfileTypeInternal.Facebook, SocialProfileType.Facebook);
        public static readonly TypeMapping<SocialProfileTypeInternal, SocialProfileType> LinkedIn = new TypeMapping<SocialProfileTypeInternal, SocialProfileType>(SocialProfileTypeInternal.LinkedIn, SocialProfileType.LinkedIn);
        public static readonly TypeMapping<SocialProfileTypeInternal, SocialProfileType> Flickr = new TypeMapping<SocialProfileTypeInternal, SocialProfileType>(SocialProfileTypeInternal.Flickr, SocialProfileType.Flickr);
        public static readonly TypeMapping<SocialProfileTypeInternal, SocialProfileType> Myspace = new TypeMapping<SocialProfileTypeInternal, SocialProfileType>(SocialProfileTypeInternal.Myspace, SocialProfileType.Myspace);
        public static readonly TypeMapping<SocialProfileTypeInternal, SocialProfileType> SinaWeibo = new TypeMapping<SocialProfileTypeInternal, SocialProfileType>(SocialProfileTypeInternal.SinaWeibo, SocialProfileType.SinaWeibo);

        private static readonly TypeMapping<SocialProfileTypeInternal, SocialProfileType>[] _typeMappings = new[]
        {
            Twitter,
            Facebook,
            LinkedIn,
            Flickr ,
            Myspace,
            SinaWeibo
        };

        public static SocialProfileType GetType(SocialProfileTypeInternal typeInternal)
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

        public static SocialProfileTypeInternal GetType(SocialProfileType type)
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
}