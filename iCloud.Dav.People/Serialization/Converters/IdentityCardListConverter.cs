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
    internal sealed class IdentityCardListConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

            var multiStatus = (MultiStatus)value;
            var collectionResponse = multiStatus.Responses.FirstOrDefault(x => (x.ResourceType?.Count == 1 && x.ResourceType?.FirstOrDefault()?.Name == "collection") || !Path.HasExtension(x.Href));
            var cardResponses = multiStatus.Responses.Where(x => Path.HasExtension(x.Href));

            var identityCardList = new IdentityCardList()
            {
                Kind = "resources",
                ETag = collectionResponse?.Etag,
                NextSyncToken = collectionResponse?.SyncToken ?? multiStatus.SyncToken,
                Items = multiStatus.Responses.Except(new HashSet<Response>(cardResponses) { collectionResponse }).Select(ToIdentityCard).ToList(),
            };

            if (!identityCardList.Items.Any())
            {
                identityCardList.Items.Add(new IdentityCard()
                {
                    ETag = collectionResponse?.Etag,
                    NextSyncToken = collectionResponse?.SyncToken ?? multiStatus.SyncToken,
                    Items = cardResponses.Select(ToCloudComponent).ToList()
                });
            }

            return identityCardList;
        }

        public static IdentityCard ToIdentityCard(Response response)
        {
            var resourceName = response.Href.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
            return new IdentityCard()
            {
                ResourceName = resourceName,
                Url = response.Href,
                ETag = response.Etag,
                NextSyncToken = response.SyncToken
            };
        }

        public static CloudComponent ToCloudComponent(Response response)
        {
            CloudComponent contactGroup;
            var id = Path.GetFileNameWithoutExtension(response.Href);

            if (response.Status == Status.NotFound)
            {
                contactGroup = new CloudComponent
                {
                    Id = id,
                    Uid = null,
                    Deleted = true,
                    ETag = response.Etag
                };
                return contactGroup;
            }
            else if (response.AddressData is null)
            {
                contactGroup = new CloudComponent
                {
                    Id = id,
                    Uid = null,
                    ETag = response.Etag
                };
                return contactGroup;
            }

            contactGroup = response.AddressData.Value.Deserialize<CloudComponent>();
            contactGroup.ETag = response.Etag;
            contactGroup.Id = id;
            return contactGroup;
        }
    }
}