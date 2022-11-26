namespace iCloud.Dav.Core
{
    /// <summary>
    /// HTTP client factory creates configurable HTTP clients. A unique HTTP client should be created for each service.
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>Creates a new configurable HTTP client.</summary>
        ConfigurableHttpClient CreateHttpClient(CreateHttpClientArgs args);
    }
}