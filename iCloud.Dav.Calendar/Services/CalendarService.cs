using iCloud.Dav.Calendar.Resources;
using iCloud.Dav.Core;

namespace iCloud.Dav.Calendar.Services
{
    /// <summary>
    /// Represents the service to interact with the Apple iCloud Calendar.
    /// </summary>
    public class CalendarService : BaseClientService
    {
        /// <summary>
        /// The calendar service version.
        /// </summary>
        public const string Version = "v1";

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarService"/> class.
        /// </summary>
        /// <param name="initializer">The initializer to use for the service.</param>
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
        /// Gets the <see cref="EventsResource"/>.
        /// </summary>
        public virtual EventsResource Events { get; }

        /// <summary>
        /// Gets the <see cref="RemindersResource"/>.
        /// </summary>
        public virtual RemindersResource Reminders { get; }

        /// <summary>
        /// Gets the <see cref="CalendarListResource"/>.
        /// </summary>
        public virtual CalendarListResource Calendars { get; }
    }
}