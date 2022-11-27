using iCloud.Dav.Core;
using vCard.Net.CardComponents;

namespace iCloud.Dav.People.DataTypes
{
    public class CloudComponent : UniqueComponent, IDirectResponseSchema
    {
        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        public virtual string Id { get; set; }

        public virtual bool? Deleted { get; set; }
    }
}