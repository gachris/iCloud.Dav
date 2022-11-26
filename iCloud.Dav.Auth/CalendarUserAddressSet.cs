using Newtonsoft.Json;

namespace iCloud.Dav.Auth
{
    /// <summary>
    /// Calendar user address set
    /// </summary>
    public class CalendarUserAddressSet
    {
        /// <summary>
        /// Gets or sets url.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; }

        /// <summary>
        /// Gets or sets preferred.
        /// </summary>
        [JsonProperty("preferred")]
        public bool Preferred { get; }

        public CalendarUserAddressSet(string url, bool preferred)
        {
            Url = url;
            Preferred = preferred;
        }
    }
}