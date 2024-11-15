using System.ComponentModel;
using iCloud.Dav.Calendar.Serialization.Converters;
using iCloud.Dav.Calendar.WebDav.DataTypes;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;

namespace iCloud.Dav.Calendar.DataTypes;

/// <summary>
/// Represents a list of <see cref="CalendarListEntry"/> objects.
/// </summary>
[XmlDeserializeType(typeof(MultiStatus))]
[TypeConverter(typeof(CalendarListConverter))]
public class CalendarList : IDirectResponseSchema
{
    /// <summary>
    /// Gets or sets the ETag of the collection.
    /// </summary>
    public virtual string ETag { get; set; }

    /// <summary>
    /// Gets or sets the list of CalendarListEntry.
    /// </summary>
    public virtual IList<CalendarListEntry> Items { get; set; }

    /// <summary>
    /// Gets or sets the token used at a later point in time to retrieve only the entries that have changed
    /// since this result was returned.
    /// </summary>
    public virtual string NextSyncToken { get; set; }
}