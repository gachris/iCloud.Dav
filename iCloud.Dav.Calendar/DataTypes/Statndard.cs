using Ical.Net.CalendarComponents;

namespace iCloud.Dav.Calendar.DataTypes
{
    /// <summary>
    /// Represents a standard time zone component in an iCalendar file.
    /// </summary>
    public class Statndard : CalendarComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Statndard"/> class with the "STANDARD" component name.
        /// </summary>
        public Statndard() : base("STANDARD")
        {
        }
    }
}