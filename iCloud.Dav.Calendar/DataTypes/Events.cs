using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.Core.WebDav.Cal;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <summary>
    /// Represents a collection of events on a calendar.
    /// </summary>
    [TypeConverter(typeof(EventListConverter))]
    [XmlDeserializeType(typeof(MultiStatus))]
    public class Events : IDirectResponseSchema
    {
        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        /// <summary>
        /// Gets or sets the list of events on the calendar.
        /// </summary>
        public virtual IList<Event> Items { get; set; }

        /// <summary>
        /// Gets or sets the token used to access the next page of this result. Omitted if no further results
        /// are available, in which case nextSyncToken is provided.
        /// </summary>
        public virtual string NextSyncToken { get; set; }

        /// <summary>
        /// Gets or sets the type of the collection ("#events").
        /// </summary>
        public virtual string Kind { get; set; }
    }
}