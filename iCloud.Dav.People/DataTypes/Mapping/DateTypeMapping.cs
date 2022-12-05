using iCloud.Dav.People.Utils;

namespace iCloud.Dav.People.DataTypes.Mapping
{
    internal class DateTypeMapping
    {
        public static readonly TypeMapping<DateTypeInternal, DateType> Other = new TypeMapping<DateTypeInternal, DateType>(DateTypeInternal.Other, DateType.Other);

        private static readonly TypeMapping<DateTypeInternal, DateType>[] _typeMappings = new[]
        {
            Other,
        };

        public static DateType GetType(DateTypeInternal typeInternal)
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

        public static DateTypeInternal GetType(DateType type)
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