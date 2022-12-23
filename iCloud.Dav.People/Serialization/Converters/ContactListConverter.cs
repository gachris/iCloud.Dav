using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace iCloud.Dav.People.Serialization.Converters
{
    internal sealed class ContactListConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

            var multiStatus = (MultiStatus)value;
            var addressbook = multiStatus.Responses.FirstOrDefault(x => x.ResourceType?.Any(resourceType => resourceType.Name == "addressbook") == true || !Path.HasExtension(x.Href.TrimEnd('/')));
            var responses = multiStatus.Responses.Where(response => !response.AddressData.Value.Contains("X-ADDRESSBOOKSERVER-KIND:group"));

            return new ContactList()
            {
                Kind = "contacts",
                Items = responses.Except(new HashSet<Response>() { addressbook }).Select(ToContact).ToList()
            };
        }

        private static Contact ToContact(Response response)
        {
            var bytes = Encoding.UTF8.GetBytes(response.AddressData.Value);
            using (var stream = new MemoryStream(bytes))
            {
                var contact = ContactDeserializer.Default.Deserialize(new StreamReader(stream, Encoding.UTF8)).First();
                contact.ETag = response.Etag;
                contact.Id = Path.GetFileNameWithoutExtension(response.Href);
                return contact;
            }
        }
    }
}