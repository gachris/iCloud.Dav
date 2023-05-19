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
        private const string ResourcesKind = "resources";

        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

            var multiStatus = (MultiStatus)value;
            var collectionResponse = multiStatus.Responses.FirstOrDefault(response => response.IsCollection());

            var identityCardList = new IdentityCardList()
            {
                Kind = ResourcesKind,
                ETag = collectionResponse?.Etag,
                MeCard = Path.GetFileNameWithoutExtension(collectionResponse.MeCard?.Value?.TrimEnd('/')),
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

        private static IdentityCard ToIdentityCard(Response response)
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