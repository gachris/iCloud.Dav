using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Logger;
using System.Net.Http;

namespace iCloud.Dav.Core;

/// <summary>
/// The default implementation of the HTTP client factory.
/// </summary>
public class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// The class logger.
    /// </summary>
    private static readonly ILogger _logger = ApplicationContext.Logger.ForType<HttpClientFactory>();

    /// <inheritdoc/>
    public ConfigurableHttpClient CreateHttpClient(CreateHttpClientArgs args)
    {
        var httpClient = new ConfigurableHttpClient(new ConfigurableMessageHandler(CreateHandler(args))
        {
            ApplicationName = args.ApplicationName
        });
        args.Initializers.ForEach(initializer => initializer.Initialize(httpClient));
        return httpClient;
    }

    /// <summary>
    /// Creates a HTTP message handler. Override this method to mock a message handler.
    /// </summary>
    protected virtual HttpMessageHandler CreateHandler(CreateHttpClientArgs args)
    {
        var httpClientHandler = new HttpClientHandler();
        if (httpClientHandler.SupportsRedirectConfiguration)
            httpClientHandler.AllowAutoRedirect = false;
        _logger.Debug("Handler was created. SupportsRedirectConfiguration={0}, SupportsAutomaticDecompression={1}", httpClientHandler.SupportsRedirectConfiguration, httpClientHandler.SupportsAutomaticDecompression);
        return httpClientHandler;
    }
}