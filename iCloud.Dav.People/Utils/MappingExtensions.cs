using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Serialization;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.Dav.People.Utils
{
    internal static class MappingExtensions
    {
        public static ContactGroup DeserializeContactGroup(this string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            using (var stream = new MemoryStream(bytes))
            {
                return ContactGroupDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).First();
            }
        }

        public static Contact DeserializeContact(this string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            using (var stream = new MemoryStream(bytes))
            {
                return ContactDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).First();
            }
        }

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