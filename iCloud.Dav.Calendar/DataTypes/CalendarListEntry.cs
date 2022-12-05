using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <summary>
    /// Represents a calendar component, a component with a unique id,
    /// which can be used to uniquely identify the component.    
    /// </summary>
    [XmlDeserializeType(typeof(MultiStatus))]
    [TypeConverter(typeof(CalendarConverter))]
    public class CalendarListEntry : IDirectResponseSchema
    {
        /// <summary>
        /// Id of the calendar.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Summary of the calendar.
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// Description of the calendar. Optional.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Color of the calendar. Optional.
        /// </summary>
        public virtual string Color { get; set; }

        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        /// <summary>
        /// Id of the calendar.
        /// </summary>
        public virtual string CTag { get; set; }

        /// <summary>
        /// A collection of privilege for the calendar.
        /// </summary>
        public virtual List<string> Privileges { get; }

        /// <summary>
        /// A collection of supported reports for the calendar.
        /// </summary>
        public virtual List<string> SupportedReports { get; }

        /// <summary>
        /// A collection of supported calendar components for the calendar.
        /// </summary>
        public virtual List<string> SupportedCalendarComponents { get; }

        /// <summary>
        /// TimeZone of the calendar. Optional.
        /// </summary>
        public virtual VTimeZone TimeZone { get; set; }

        /// <summary>
        /// Order of the calendar. Optional.
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Type of the collection ("calendar#calendarList").
        /// </summary>
        public virtual string Kind { get; set; }

        public CalendarListEntry()
        {
            Privileges = new List<string>();
            SupportedReports = new List<string>();
            SupportedCalendarComponents = new List<string>();
            EnsureProperties();
        }

        private void EnsureProperties()
        {
            if (string.IsNullOrEmpty(Id))
            {
                // Create a new ID for the component
                Id = Guid.NewGuid().ToString();
            }
        }
    }
}