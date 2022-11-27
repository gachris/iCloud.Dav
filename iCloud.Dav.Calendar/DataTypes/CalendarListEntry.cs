﻿using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.DataTypes
{
    [XmlDeserializeType(typeof(MultiStatus))]
    [TypeConverter(typeof(CalendarConverter))]
    public class CalendarListEntry : IDirectResponseSchema
    {
        public CalendarListEntry()
        {
            Privileges = new List<string>();
            SupportedReports = new List<string>();
            SupportedCalendarComponents = new List<string>();
        }

        public virtual string Uid { get; set; }

        public virtual string Summary { get; set; }

        /// <summary>
        /// Description of the calendar. Optional. Read-only.
        /// </summary>
        public virtual string Description { get; set; }

        public virtual string Color { get; set; }

        /// <inheritdoc/>
        public virtual string ETag { get; set; }

        public virtual string CTag { get; set; }

        public virtual List<string> Privileges { get; }

        public virtual List<string> SupportedReports { get; }

        public virtual List<string> SupportedCalendarComponents { get; }

        public virtual VTimeZone TimeZone { get; set; }

        /// <summary>
        /// Whether this calendar list entry has been deleted from the calendar list. Read-only.
        /// Optional. The default is False.
        /// </summary>
        public virtual bool? Deleted { get; set; }

        public string Order { get; set; }

        /// <summary>
        /// Type of the collection ("calendar#calendarList").
        /// </summary>
        public virtual string Kind { get; set; }

        public string Href { get; internal set; }
    }
}