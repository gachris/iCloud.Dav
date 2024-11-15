namespace iCloud.Dav.Core;

/// <inheritdoc/>
public class ConfigurableHttpClient : HttpClient
{
    /// <summary>Gets the configurable message handler.</summary>
    public ConfigurableMessageHandler MessageHandler { get; private set; }

    /// <summary>Constructs a new HTTP client.</summary>
    public ConfigurableHttpClient(ConfigurableMessageHandler handler) : base(handler)
    {
        MessageHandler = handler;
        DefaultRequestHeaders.ExpectContinue = new bool?(false);
    }
}