using iCloud.Dav.Core;
using System.Collections.Generic;

namespace iCloud.Dav.People.DataTypes
{
    public class IdentityCard : IDirectResponseSchema
    {
        /// <summary>
        /// The resource name of the IdentityCard
        /// </summary>
        public virtual string ResourceName { get; set; }

        /// <summary>
        /// The url of the IdentityCard
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// ETag of the collection.
        /// </summary>
        public virtual string ETag { get; set; }

        /// <summary>
        /// A token, which can be sent as `sync_token` to retrieve changes since the last
        /// request. Request must set `request_sync_token` to return the sync token. When
        /// the response is paginated, only the last page will contain `nextSyncToken`.
        /// </summary>
        public virtual string NextSyncToken { get; set; }

        /// <summary>
        /// Items of the collection.
        /// </summary>
        public virtual IList<CloudComponent> Items { get; set; }
    }
}