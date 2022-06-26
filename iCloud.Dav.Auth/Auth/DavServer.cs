using Newtonsoft.Json;

namespace iCloud.Dav.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class DavServer
    {
        /// <summary>
        /// 
        /// </summary>
        public DavServer()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="url"></param>
        public DavServer(string id, string url)
        {
            Id = id;
            Url = url;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
