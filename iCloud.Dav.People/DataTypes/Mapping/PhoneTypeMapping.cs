using iCloud.Dav.People.Utils;

namespace iCloud.Dav.People.DataTypes.Mapping
{
    internal class PhoneTypeMapping
    {
        public static readonly TypeMapping<PhoneTypeInternal, PhoneType> iPhone = new TypeMapping<PhoneTypeInternal, PhoneType>(PhoneTypeInternal.iPhone | PhoneTypeInternal.Cell | PhoneTypeInternal.Voice, PhoneType.iPhone);
        public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Home = new TypeMapping<PhoneTypeInternal, PhoneType>(PhoneTypeInternal.Home | PhoneTypeInternal.Voice, PhoneType.Home);
        public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Work = new TypeMapping<PhoneTypeInternal, PhoneType>(PhoneTypeInternal.Work | PhoneTypeInternal.Voice, PhoneType.Work);
        public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Main = new TypeMapping<PhoneTypeInternal, PhoneType>(PhoneTypeInternal.Main, PhoneType.Main);
        public static readonly TypeMapping<PhoneTypeInternal, PhoneType> HomeFax = new TypeMapping<PhoneTypeInternal, PhoneType>(PhoneTypeInternal.Fax | PhoneTypeInternal.Home, PhoneType.HomeFax);
        public static readonly TypeMapping<PhoneTypeInternal, PhoneType> WorkFax = new TypeMapping<PhoneTypeInternal, PhoneType>(PhoneTypeInternal.Fax | PhoneTypeInternal.Work, PhoneType.WorkFax);
        public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Pager = new TypeMapping<PhoneTypeInternal, PhoneType>(PhoneTypeInternal.Pager, PhoneType.Pager);
        public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Other = new TypeMapping<PhoneTypeInternal, PhoneType>(PhoneTypeInternal.Voice | PhoneTypeInternal.Other, PhoneType.Other);
        public static readonly TypeMapping<PhoneTypeInternal, PhoneType> Mobile = new TypeMapping<PhoneTypeInternal, PhoneType>(PhoneTypeInternal.Cell | PhoneTypeInternal.Voice, PhoneType.Mobile);

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
}