using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

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
            var addressbook = multiStatus.Responses.FirstOrDefault(x => x.ResourceType?.Any(resourceType => resourceType.Name == "addressbook") == true || !Path.HasExtension(x.Href));

            return new ContactList()
            {
                Kind = "contacts",
                ETag = addressbook?.Etag,
                NextSyncToken = addressbook?.SyncToken ?? multiStatus.SyncToken,
                Items = multiStatus.Responses.Except(new HashSet<Response>() { addressbook }).Select(ToContact).ToList()
            };
        }

        private static Contact ToContact(Response response)
        {
            Contact contact;
            var id = Path.GetFileNameWithoutExtension(response.Href);

            if (response.Status == Status.NotFound)
            {
                contact = new Contact
                {
                    Id = id,
                    Uid = null,
                    Deleted = true,
                    ETag = response.Etag
                };
                return contact;
            }
            else if (response.AddressData is null)
            {
                contact = new Contact
                {
                    Id = id,
                    Uid = null,
                    ETag = response.Etag
                };
                return contact;
            }

            contact = response.AddressData.Value.Deserialize<Contact>();
            contact.ETag = response.Etag;
            contact.Id = id;
            return contact;
        }
    }
}