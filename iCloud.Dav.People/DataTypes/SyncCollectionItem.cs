using iCloud.Dav.Core;

namespace iCloud.Dav.People.DataTypes
{
    public class SyncCollectionItem : IDirectResponseSchema
    {
        public string Id { get; set; }

        /// <summary>
        /// ETag of the collection.
        /// </summary>
        public virtual string ETag { get; set; }

        /// <summary>
        /// Whether this object has been deleted from the list. Read-only.
        /// Optional. The default is False.
        /// </summary>
        public virtual bool? Deleted { get; set; }
    }
}