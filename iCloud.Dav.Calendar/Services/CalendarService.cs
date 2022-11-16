using iCloud.Dav.Calendar.Resources;
using iCloud.Dav.Core;

namespace iCloud.Dav.Calendar.Services;

public class CalendarService : BaseClientService
{
    public const string Version = "v1";

    public CalendarService(Initializer initializer) : base(initializer)
    {
        Events = new EventsResource(this);
        Reminders = new RemindersResource(this);
        Calendars = new CalendarsResource(this);
        BasePath = initializer.HttpClientInitializer.GetUri(PrincipalHomeSet.Calendar);
    }

    public override string Name => "calendar";

    public override string BasePath { get; }

    /// <summary>Gets the Events resource.</summary>
    public virtual EventsResource Events { get; }

    /// <summary>Gets the Reminders resource.</summary>
    public virtual RemindersResource Reminders { get; }

    /// <summary>Gets the Calendars resource.</summary>
    public virtual CalendarsResource Calendars { get; }
}