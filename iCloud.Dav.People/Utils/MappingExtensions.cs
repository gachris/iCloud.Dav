using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using vCard.Net.CardComponents;

namespace iCloud.Dav.People.Utils
{
    internal static class MappingExtensions
    {
        public static T Deserialize<T>(this string data) where T : ICardComponent
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            using (var stream = new MemoryStream(bytes))
            {
                return (T)CardDeserializer.Default.Deserialize<T>(new StreamReader(stream, Encoding.UTF8)).First();
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