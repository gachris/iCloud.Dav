using System.Net.Http;

namespace iCloud.Dav.Core.Services
{
    public class ConfigurableHttpClient : HttpClient
    {
        /// <summary>Gets the configurable message handler.</summary>
        public ConfigurableMessageHandler MessageHandler { get; private set; }

        /// <summary>Constructs a new HTTP client.</summary>
        public ConfigurableHttpClient(ConfigurableMessageHandler handler) : base((HttpMessageHandler)handler)
        {
            this.MessageHandler = handler;
            this.DefaultRequestHeaders.ExpectContinue = (new bool?(false));
        }
    }
}
