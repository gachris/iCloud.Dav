using Newtonsoft.Json;

namespace iCloud.Dav.Auth;

/// <summary>
/// Represents a user address set for a calendar user, which may include one or more calendar user addresses.
/// </summary>
public class CalendarUserAddressSet
{
    /// <summary>
    /// Gets or sets the URL of the calendar user address set.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this is the preferred calendar user address set for the user.
    /// </summary>
    [JsonProperty("preferred")]
    public bool Preferred { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CalendarUserAddressSet"/> class with the specified parameters.
    /// </summary>
    /// <param name="url">The URL of the calendar user address set.</param>
    /// <param name="preferred">A value indicating whether this is the preferred calendar user address set for the user.</param>
    public CalendarUserAddressSet(string url, bool preferred)
    {
        Url = url;
        Preferred = preferred;
    }
}