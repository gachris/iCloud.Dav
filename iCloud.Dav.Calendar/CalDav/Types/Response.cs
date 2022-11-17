using System.Collections.Generic;

namespace iCloud.Dav.Calendar.CalDav.Types;

internal sealed class Response
{
    public string Href { get; }

    public string? DisplayName { get; }

    public string? CalendarColor { get; }

    public string? Etag { get; }

    public Attribute? Ctag { get; }

    public Attribute? CalendarData { get; }

    public IReadOnlyList<Privilege> CurrentUserPrivilegeSet { get; }

    public IReadOnlyList<SupportedReport> SupportedReportSet { get; }

    public IReadOnlyList<CalendarComponent> SupportedCalendarComponentSet { get; }
    
    public string? SyncToken { get; }

    public Response(
        string href,
        string? displayName,
        string? calendarColor,
        string? etag,
        Attribute? ctag,
        Attribute? calendarData,
        IReadOnlyList<Privilege> currentUserPrivilegeSet,
        IReadOnlyList<SupportedReport> supportedReportSet,
        IReadOnlyList<CalendarComponent> supportedCalendarComponentSet,
        string? syncToken)
    {
        Href = href;
        DisplayName = displayName;
        CalendarColor = calendarColor;
        Etag = etag;
        Ctag = ctag;
        CalendarData = calendarData;
        CurrentUserPrivilegeSet = currentUserPrivilegeSet;
        SupportedReportSet = supportedReportSet;
        SupportedCalendarComponentSet = supportedCalendarComponentSet;
        SyncToken = syncToken;
    }
}
