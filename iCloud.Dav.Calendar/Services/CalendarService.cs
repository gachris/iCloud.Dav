using iCloud.Dav.Calendar.Resources;
using iCloud.Dav.Core;

namespace iCloud.Dav.Calendar.Services
{
    /// <summary>
    /// The calendar service collection of resources.
    /// </summary>
    public class CalendarService : BaseClientService
    {
        public const string Version = "v1";

        /// <summary>
        /// Constructs a new service.
        /// </summary>
        public CalendarService(Initializer initializer) : base(initializer)
        {
            Events = new EventsResource(this);
            Reminders = new RemindersResource(this);
            Calendars = new CalendarListResource(this);
            BasePath = initializer.HttpClientInitializer.GetUri(PrincipalHomeSet.Calendar).ToString();
        }

        /// <inheritdoc/>
        public override string Name => "calendar";

        /// <inheritdoc/>
        public override string BasePath { get; }

        /// <summary>
        /// Gets the Events resource.
        /// </summary>
        public virtual EventsResource Events { get; }

        /// <summary>
        /// Gets the Reminders resource.
        /// </summary>
        public virtual RemindersResource Reminders { get; }

        /// <summary>
        /// Gets the Calendars resource.
        /// </summary>
        public virtual CalendarListResource Calendars { get; }
    }
}