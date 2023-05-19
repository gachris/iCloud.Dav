namespace iCloud.Dav.Core
{
    /// <summary>
    /// An interface for initializing a <see cref="ConfigurableHttpClient"/> after it was created.
    /// </summary>
    public interface IConfigurableHttpClientInitializer
    {
        /// <summary>
        /// Initializes a <see cref="ConfigurableHttpClient"/> by configuring its properties.
        /// </summary>
        /// <param name="httpClient">The <see cref="ConfigurableHttpClient"/> instance to be initialized.</param>
        void Initialize(ConfigurableHttpClient httpClient);
    }
}