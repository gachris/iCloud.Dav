using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
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

            var responses = ((MultiStatus)value).Responses;
            var collectionResponse = responses.FirstOrDefault(x => (x.ResourceType?.Count == 1 && x.ResourceType?.FirstOrDefault()?.Name == "collection") ||
                                                                   x.ResourceType?.Any(resourceType => resourceType.Name == "addressbook") == true || 
                                                                   !Path.HasExtension(x.Href.TrimEnd('/')));

            return new SyncCollectionList()
            {
                NextSyncToken = collectionResponse?.SyncToken,
                ETag = collectionResponse?.Etag,
                Items = responses.Except(new HashSet<Response>() { collectionResponse }).Select(ToSyncCollectionItem).ToList()
            };
        }

        private static SyncCollectionItem ToSyncCollectionItem(Response response)
        {
            return new SyncCollectionItem()
            {
                Id = Path.GetFileNameWithoutExtension(response.Href.TrimEnd('/')),
                ETag = response.Etag,
                Deleted = response.Status == Status.NotFound ? true : (bool?)null
            };
        }
    }

}