using Ical.Net.CalendarComponents;

namespace iCloud.Dav.Calendar.DataTypes;

/// <summary>
/// Represents a daylight component of a calendar event.
/// </summary>
public class Daylight : CalendarComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Daylight"/> class with the component name set to "DAYLIGHT".
    /// </summary>
    public Daylight() : base("DAYLIGHT")
    {
    }
}