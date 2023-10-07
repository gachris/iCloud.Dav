using Newtonsoft.Json;

namespace iCloud.Dav.Auth;

/// <summary>
/// Represents a DAV server, which includes an ID and a URL.
/// </summary>
public class DavServer
{
    /// <summary>
    /// Gets the ID of the DAV server.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; }

    /// <summary>
    /// Gets the URL of the DAV server.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DavServer"/> class with the specified parameters.
    /// </summary>
    /// <param name="id">The ID of the DAV server.</param>
    /// <param name="url">The URL of the DAV server.</param>
    public DavServer(string id, string url)
    {
        Id = id;
        Url = url;
    }
}