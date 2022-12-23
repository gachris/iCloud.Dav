﻿using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace iCloud.Dav.People.Serialization.Converters
{
    internal sealed class SyncTokenConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(MultiStatus);

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!CanConvertFrom(context, value.GetType())) throw GetConvertFromException(value);

            var multiStatus = (MultiStatus)value;
            var collectionResponse = multiStatus.Responses.FirstOrDefault(x => (x.ResourceType?.Count == 1 && x.ResourceType?.FirstOrDefault()?.Name == "collection")
                                                                               || x.ResourceType?.Any(resourceType => resourceType.Name == "addressbook") == true
                                                                               || !Path.HasExtension(x.Href.TrimEnd('/')));

            return new SyncToken()
            {
                ETag = collectionResponse.Etag,
                NextSyncToken = collectionResponse.SyncToken
            };
        }
    }

}