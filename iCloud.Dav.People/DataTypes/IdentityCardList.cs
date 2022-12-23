using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Serialization.Converters;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Represents a strongly typed list of objects that can be accessed by index. Provides
    /// methods to search, sort, and manipulate lists.
    /// </summary>   
    [TypeConverter(typeof(IdentityCardListConverter))]
    [XmlDeserializeType(typeof(MultiStatus))]
    public class IdentityCardList : IDirectResponseSchema
    {
        /// <summary>
        /// ETag of the collection.
        /// </summary>
        public virtual string ETag { get; set; }

        /// <summary>
        /// Items of the collection.
        /// </summary>
        public virtual IList<IdentityCard> Items { get; set; }

        /// <summary>
        /// A token, which can be sent as `sync_token` to retrieve changes since the last
        /// request. Request must set `request_sync_token` to return the sync token. When
        /// the response is paginated, only the last page will contain `nextSyncToken`.
        /// </summary>
        public virtual string NextSyncToken { get; set; }

        /// <summary>
        /// Type of the collection ("people#resource").
        /// </summary>
        public virtual string Kind { get; set; }

        /// <summary>
        /// Me card of the collection.
        /// </summary>
        public virtual string MeCard { get; set; }
    }
}