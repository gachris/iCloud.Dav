using Newtonsoft.Json;

namespace iCloud.Dav.Auth;

public class DavServer
{
    [JsonProperty("id")]
    public string Id { get; }

    [JsonProperty("url")]
    public string Url { get; }

    public DavServer(string id, string url) => (Id, Url) = (id, url);
}
