using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.CalDav.Types;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Dav.Calendar.Data;

[XmlDeserializeType(typeof(MultiStatus))]
[TypeConverter(typeof(CalendarConverter))]
public class Calendar : IDirectResponseSchema
{
    public Calendar()
    {
        Privileges = new List<string>();
        SupportedReports = new List<string>();
        SupportedCalendarComponents = new List<string>();
    }

    public virtual string? Uid { get; set; }

    public virtual string? Summary { get; set; }

    public virtual string? Color { get; set; }

    /// <inheritdoc/>
    public virtual string? ETag { get; set; }

    public virtual string? CTag { get; set; }

    public virtual List<string> Privileges { get; }

    public virtual List<string> SupportedReports { get; }

    public virtual List<string> SupportedCalendarComponents { get; }

    public virtual VTimeZone? TimeZone { get; set; }

    public string? SyncToken { get; internal set; }
}