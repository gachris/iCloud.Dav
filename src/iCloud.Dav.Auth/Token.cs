using Newtonsoft.Json;
using System;

namespace iCloud.Dav.Auth;

/// <summary>
/// Represents a user token, which includes access to calendar and people server resources.
/// </summary>
public class Token
{
    /// <summary>
    /// Gets or sets the calendar server information for the user.
    /// </summary>
    [JsonProperty("calendar_server")]
    public DavServer CalendarServer { get; }

    /// <summary>
    /// Gets or sets the calendar principal information for the user.
    /// </summary>
    [JsonProperty("calendar_principal")]
    public CalendarPrincipal CalendarPrincipal { get; }

    /// <summary>
    /// Gets or sets the people server information for the user.
    /// </summary>
    [JsonProperty("people_server")]
    public DavServer PeopleServer { get; }

    /// <summary>
    /// Gets or sets the people principal information for the user.
    /// </summary>
    [JsonProperty("people_principal")]
    public PeoplePrincipal PeoplePrincipal { get; }

    /// <summary>
    /// Gets or sets the access token for the user.
    /// </summary>
    [JsonProperty("access_token")]
    public string AccessToken { get; }

    /// <summary>
    /// Gets the date and time that this token was issued.
    /// </summary>
    [JsonProperty("issued")]
    public DateTime Issued { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class with the specified parameters.
    /// </summary>
    /// <param name="calendarServer">The calendar server information for the user.</param>
    /// <param name="calendarPrincipal">The calendar principal information for the user.</param>
    /// <param name="peopleServer">The people server information for the user.</param>
    /// <param name="peoplePrincipal">The people principal information for the user.</param>
    /// <param name="accessToken">The access token for the user.</param>
    /// <param name="issued">The date and time that this token was issued.</param>
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