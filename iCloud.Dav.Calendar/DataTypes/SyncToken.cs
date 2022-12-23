﻿using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <inheritdoc/>
    [XmlDeserializeType(typeof(MultiStatus))]
    [TypeConverter(typeof(SyncTokenConverter))]
    public class SyncToken : IDirectResponseSchema
    {
        /// <inheritdoc/>
        public string ETag { get; set; }

        /// <summary>
        /// Token used to retrieve only the entries that have changed
        /// since this result was returned.
        /// </summary>
        public virtual string NextSyncToken { get; set; }
    }
}