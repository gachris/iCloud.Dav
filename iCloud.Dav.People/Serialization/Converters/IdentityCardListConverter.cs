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
            var collectionResponse = multiStatus.Responses.FirstOrDefault(x => (x.ResourceType?.Count == 1 && x.ResourceType?.FirstOrDefault()?.Name == "collection") || !Path.HasExtension(x.Href.TrimEnd('/')));

            var identityCardList = new IdentityCardList()
            {
                Kind = "resources",
                ETag = collectionResponse?.Etag,
                NextSyncToken = collectionResponse?.SyncToken ?? multiStatus.SyncToken,
                Items = multiStatus.Responses.Except(new HashSet<Response>() { collectionResponse }).Select(ToIdentityCard).ToList(),
            };

            if (!identityCardList.Items.Any())
            {
                identityCardList.Items.Add(new IdentityCard()
                {
                    ETag = collectionResponse?.Etag,
                    NextSyncToken = collectionResponse?.SyncToken ?? multiStatus.SyncToken,
                });
            }

            return identityCardList;
        }

        public static IdentityCard ToIdentityCard(Response response)
        {
            return new IdentityCard()
            {
                ResourceName = Path.GetFileNameWithoutExtension(response.Href.TrimEnd('/')),
                ETag = response.Etag,
                NextSyncToken = response.SyncToken
            };
        }
    }
}