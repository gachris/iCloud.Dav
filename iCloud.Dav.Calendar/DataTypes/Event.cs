using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <inheritdoc/>
    [TypeConverter(typeof(EventConverter))]
    public class Event : CalendarEvent, IDirectResponseSchema
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string Id { get; set; }

        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        /// <summary>
        /// Whether this event has been deleted from the calendar. Read-only.
        /// Optional. The default is False.
        /// </summary>
        public virtual bool? Deleted { get; set; }
    }
}