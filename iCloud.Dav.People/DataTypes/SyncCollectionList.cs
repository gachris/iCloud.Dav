using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Serialization.Converters;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People.DataTypes
{
    /// <inheritdoc/>
    [XmlDeserializeType(typeof(MultiStatus))]
    [TypeConverter(typeof(SyncCollectionListConverter))]
    public class SyncCollectionList : IDirectResponseSchema
    {
        /// <summary>
        /// ETag of the collection.
        /// </summary>
        public virtual string ETag { get; set; }

        /// <summary>
        /// Calendars that are present on the user's calendar list.
        /// </summary>
        public virtual IList<SyncCollectionItem> Items { get; set; }

        /// <summary>
        /// Token used at a later point in time to retrieve only the entries that have changed
        /// since this result was returned.
        /// </summary>
        public virtual string NextSyncToken { get; set; }
    }
}