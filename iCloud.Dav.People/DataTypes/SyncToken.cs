using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.Serialization.Converters;
using iCloud.Dav.Core.WebDav.Card;
using System.ComponentModel;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Represents a synchronization token used to retrieve only the entries that have changed since a previous sync.
    /// </summary>
    [XmlDeserializeType(typeof(MultiStatus))]
    [TypeConverter(typeof(SyncTokenConverter))]
    public class SyncToken : IDirectResponseSchema
    {
        /// <inheritdoc/>
        public string ETag { get; set; }

        /// <summary>
        /// Token used to retrieve only the entries that have changed since this result was returned.
        /// </summary>
        public virtual string NextSyncToken { get; set; }
    }
}