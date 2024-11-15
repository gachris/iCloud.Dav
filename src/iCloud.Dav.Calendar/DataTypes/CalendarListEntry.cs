using System.ComponentModel;
using Ical.Net.CalendarComponents;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Calendar.WebDav.DataTypes;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;

namespace iCloud.Dav.Calendar.DataTypes;

/// <summary>
/// Represents a calendar list entry, which is an entry on the user's calendar list containing information
/// about a specific calendar, including its unique ID, summary, description, color, and timezone.
/// </summary>
[XmlDeserializeType(typeof(MultiStatus))]
[TypeConverter(typeof(CalendarConverter))]
public class CalendarListEntry : IDirectResponseSchema
{
    /// <summary>
    /// The unique identifier of the calendar.
    /// </summary>
    public virtual string Id { get; set; }

    /// <summary>
    /// A summary of the calendar.
    /// </summary>
    public virtual string Summary { get; set; }

    /// <summary>
    /// A description of the calendar, if available.
    /// </summary>
    public virtual string Description { get; set; }

    /// <summary>
    /// The color of the calendar, if set.
    /// </summary>
    /// <remarks>
    /// The color should be specified in RGB format (#RRGGBB).
    /// </remarks>
    public virtual string Color { get; set; }

    /// <inheritdoc/>
    public virtual string ETag { get; set; }

    /// <summary>
    /// The current synchronization tag for the calendar list entry.
    /// </summary>
    public virtual string CTag { get; set; }

    /// <summary>
    /// A collection of privileges for the calendar.
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
    /// The time zone of the calendar, if set.
    /// </summary>
    public virtual VTimeZone TimeZone { get; set; }

    /// <summary>
    /// The order of the calendar in the user's calendar list.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// The type of the calendar list entry.
    /// </summary>
    public virtual string Kind { get; set; }

    /// <summary>
    /// Indicates whether the calendar list entry has been deleted.
    /// </summary>
    public bool? Deleted { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarListEntry"/> class.
    /// </summary>
    public CalendarListEntry()
    {
        Privileges = new List<string>();
        SupportedReports = new List<string>();
        SupportedCalendarComponents = new List<string>();
        EnsureProperties();
    }

    /// <summary>
    /// Ensures that the properties of the calendar list entry are set.
    /// </summary>
    private void EnsureProperties()
    {
        if (string.IsNullOrEmpty(Id))
        {
            // Generate a new ID for the calendar list entry.
            Id = Guid.NewGuid().ToString();
        }
    }
}