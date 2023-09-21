using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.Core.WebDav.Cal;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <summary>
    /// A list of SyncCollectionItem objects.
    /// </summary>
    [XmlDeserializeType(typeof(MultiStatus))]
    [TypeConverter(typeof(SyncCollectionListConverter))]
    public class SyncCollectionList : IDirectResponseSchema
    {
        /// <summary>
        /// ETag of the collection.
        /// </summary>
        public virtual string ETag { get; set; }

        /// <summary>
        /// The list of SyncCollectionItem objects.
        /// </summary>
        public virtual IList<SyncCollectionItem> Items { get; set; }

        /// <summary>
        /// Token used at a later point in time to retrieve only the entries that have changed
        /// since this result was returned.
        /// </summary>
        public virtual string NextSyncToken { get; set; }
    }
}