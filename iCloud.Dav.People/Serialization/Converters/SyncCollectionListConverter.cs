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
    internal sealed class SyncCollectionListConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

            var multiStatus = (MultiStatus)value;
            var syncCollectionListResponse = multiStatus.Responses.FirstOrDefault(response => response.IsCollection() || response.IsAddressbook());

            return new SyncCollectionList()
            {
                NextSyncToken = syncCollectionListResponse?.SyncToken ?? multiStatus.SyncToken,
                Items = multiStatus.Responses.Except(new HashSet<Response>() { syncCollectionListResponse }).Select(ToSyncCollectionItem).ToList()
            };
        }

        private static SyncCollectionItem ToSyncCollectionItem(Response response)
        {
            return new SyncCollectionItem()
            {
                Id = Path.GetFileNameWithoutExtension(response.Href.TrimEnd('/')),
                ETag = response.Etag,
                Deleted = response.Status == System.Net.HttpStatusCode.NotFound ? true : (bool?)null
            };
        }
    }

}