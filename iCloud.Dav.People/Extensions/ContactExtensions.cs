using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Serialization;
using System.IO;
using System.Text;
using System;
using System.Linq;

namespace iCloud.Dav.People.Extensions
{
    internal static class ContactExtensions
    {
        private static readonly ContactSerializer Serializer = new ContactSerializer();

        public static Contact ToContact(this string data)
        {
            if (data.Contains(WebDavExtensions.GroupKind))
                throw new ArgumentException("Value cannot be converted to a Contact.");

            var bytes = Encoding.UTF8.GetBytes(data);
            using (var stream = new MemoryStream(bytes))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return ContactDeserializer.Default.Deserialize(reader).First();
                }
            }
        }

        public static string SerializeToString(this Contact data)
        {
            return Serializer.SerializeToString(data);
        }
    }
}