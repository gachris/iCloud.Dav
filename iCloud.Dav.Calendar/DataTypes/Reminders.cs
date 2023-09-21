using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.Core.WebDav.Cal;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <summary>
    /// Represents a collection of reminders.
    /// </summary>
    [TypeConverter(typeof(ReminderListConverter))]
    [XmlDeserializeType(typeof(MultiStatus))]
    public class Reminders : IDirectResponseSchema
    {
        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        /// <summary>
        /// List of reminders in the collection.
        /// </summary>
        public virtual IList<Reminder> Items { get; set; }

        /// <summary>
        /// Token used to access the next page of this result. Omitted if no further results
        /// are available, in which case nextSyncToken is provided.
        /// </summary>
        public virtual string NextSyncToken { get; set; }

        /// <summary>
        /// Type of the collection ("calendar#reminders").
        /// </summary>
        public virtual string Kind { get; set; }
    }
}