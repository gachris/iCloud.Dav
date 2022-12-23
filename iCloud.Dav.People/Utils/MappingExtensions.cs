using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Serialization;

namespace iCloud.Dav.People.Utils
{
    internal static class MappingExtensions
    {
        public static string SerializeToString(this ContactGroup data)
        {
            var serializer = new ContactGroupSerializer();
            return serializer.SerializeToString(data);
        }

        public static string SerializeToString(this Contact data)
        {
            var serializer = new ContactSerializer();
            return serializer.SerializeToString(data);
        }
    }
}