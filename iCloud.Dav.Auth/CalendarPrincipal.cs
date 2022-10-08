using Newtonsoft.Json;
using System.Collections.Generic;

namespace iCloud.Dav.Auth;

/// <summary>
/// Calendar principal.
/// </summary>
public class CalendarPrincipal
{
    /// <summary>
    /// Gets or sets the Current user principal.
    /// </summary>
    [JsonProperty("current_user_principal")]
    public string CurrentUserPrincipal { get; set; }

    /// <summary>
    /// Gets or sets the current user principal.
    /// </summary>
    [JsonProperty("calendar_home_set")]
    public string CalendarHomeSet { get; set; }

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    [JsonProperty("display_name")]
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the calendar user address set.
    /// </summary>
    [JsonProperty("calendar_user_address_set")]
    public List<CalendarUserAddressSet> CalendarUserAddressSet { get; set; }
}
