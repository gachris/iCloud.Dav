using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.Serialization.Converters;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Represents a strongly typed list of IdentityCard objects that can be accessed by index. Provides
    /// methods to search, sort, and manipulate lists.
    /// </summary>   
    [TypeConverter(typeof(IdentityCardListConverter))]
    [XmlDeserializeType(typeof(MultiStatus))]
    public class IdentityCardList : IDirectResponseSchema
    {
        /// <summary>
        /// The ETag of the collection.
        /// </summary>
        public virtual string ETag { get; set; }

        /// <summary>
        /// A list of <see cref="IdentityCard"/> objects in the collection.
        /// </summary>
        public virtual IList<IdentityCard> Items { get; set; }

        /// <summary>
        /// A token, which can be sent as `sync_token` to retrieve changes since the last request.
        /// </summary>
        public virtual string NextSyncToken { get; set; }

        /// <summary>
        /// Type of the collection, always set to "resource".
        /// </summary>
        public virtual string Kind { get; set; }

        /// <summary>
        /// The contact id of the me card in the collection.
        /// </summary>
        public virtual string MeCard { get; set; }
    }
}