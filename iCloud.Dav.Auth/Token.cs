using Newtonsoft.Json;
using System;

namespace iCloud.Dav.Auth;

/// <summary>
/// User token
/// </summary>
public class Token
{
    /// <summary>
    /// Gets or sets the Calendar server info.
    /// </summary>
    [JsonProperty("calendar_server")]
    public DavServer CalendarServer { get; }

    /// <summary>
    /// Gets or sets the Calendar principal info.
    /// </summary>
    [JsonProperty("calendar_principal")]
    public CalendarPrincipal CalendarPrincipal { get; }

    /// <summary>
    /// Gets or sets the People server info.
    /// </summary>
    [JsonProperty("people_server")]
    public DavServer PeopleServer { get; }

    /// <summary>
    /// Gets or sets the People principal info.
    /// </summary>
    [JsonProperty("people_principal")]
    public PeoplePrincipal PeoplePrincipal { get; }

    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    [JsonProperty("access_token")]
    public string AccessToken { get; }

    /// <summary>
    /// The date and time that this token was issued.
    /// </summary>
    [JsonProperty("issued")]
    public DateTime Issued { get; }

    public Token(DavServer calendarServer, CalendarPrincipal calendarPrincipal, DavServer peopleServer, PeoplePrincipal peoplePrincipal, string accessToken, DateTime issued)
    {
        CalendarServer = calendarServer;
        CalendarPrincipal = calendarPrincipal;
        PeopleServer = peopleServer;
        PeoplePrincipal = peoplePrincipal;
        AccessToken = accessToken;
        Issued = issued;
    }
}
