using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Converters;
using iCloud.Dav.Calendar.Types;
using iCloud.Dav.Core.Attributes;
using iCloud.Dav.Core.Services;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iCloud.Dav.Calendar
{
    [XmlDeserializeType(typeof(Multistatus<Prop>))]
    [TypeConverter(typeof(CalendarConverter))]
    public class CalendarEntry : IDirectResponseSchema, ICloneable
    {
        public CalendarEntry()
        {
            Privileges = new PrivilegeCollection();
            SupportedReports = new SupportedReportCollection();
            SupportedCalendarComponents = new SupportedCalendarComponentCollection();
        }

        [Required]
        public virtual string Id { get; set; }

        [Required]
        public virtual string Summary { get; set; }

        public virtual string Url { get; set; }

        public virtual string Color { get; set; }

        public virtual string ETag { get; set; }

        public virtual string CTag { get; set; }

        public virtual PrivilegeCollection Privileges { get; }

        public virtual SupportedReportCollection SupportedReports { get; }

        public virtual SupportedCalendarComponentCollection SupportedCalendarComponents { get; }

        public virtual VTimeZone TimeZone { get; set; }

        public object Clone() => MemberwiseClone();
    }
}