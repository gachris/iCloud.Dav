using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Core.Attributes;
using iCloud.Dav.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iCloud.Dav.Calendar
{
    [XmlDeserializeType(typeof(MultiStatus))]
    [TypeConverter(typeof(CalendarConverter))]
    public class CalendarEntry : IDirectResponseSchema, ICloneable
    {
        public CalendarEntry()
        {
            Privileges = new List<string>();
            SupportedReports = new List<string>();
            SupportedCalendarComponents = new List<string>();
        }

        [Required]
        public virtual string Id { get; set; }

        [Required]
        public virtual string Summary { get; set; }

        public virtual string Url { get; set; }

        public virtual string Color { get; set; }

        public virtual string ETag { get; set; }

        public virtual string CTag { get; set; }

        public virtual List<string> Privileges { get; }

        public virtual List<string> SupportedReports { get; }

        public virtual List<string> SupportedCalendarComponents { get; }

        public virtual VTimeZone TimeZone { get; set; }

        public object Clone() => MemberwiseClone();
    }
}