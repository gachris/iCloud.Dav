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
    internal sealed class ContactGroupListConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

            var multiStatus = (MultiStatus)value;
            var addressbook = multiStatus.Responses.FirstOrDefault(x => x.ResourceType?.Any(resourceType => resourceType.Name == "addressbook") == true || !Path.HasExtension(x.Href.TrimEnd('/')));

            return new ContactGroupList()
            {
                Kind = "groups",
                ETag = addressbook?.Etag,
                NextSyncToken = addressbook?.SyncToken ?? multiStatus.SyncToken,
                Items = multiStatus.Responses.Except(new HashSet<Response>() { addressbook }).Select(ToContactGroup).ToList()
            };
        }

        public static ContactGroup ToContactGroup(Response response)
        {
            var contactGroup = response.AddressData.Value.DeserializeContactGroup();
            contactGroup.ETag = response.Etag;
            contactGroup.Id = Path.GetFileNameWithoutExtension(response.Href);
            return contactGroup;
        }
    }
}