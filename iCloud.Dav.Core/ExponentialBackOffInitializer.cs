using System;

namespace iCloud.Dav.Core;

/// <summary>
/// An initializer which adds exponential back-off as exception handler and \ or unsuccessful response handler by
/// the given <see cref="ExponentialBackOffPolicy" />.
/// </summary>
public class ExponentialBackOffInitializer : IConfigurableHttpClientInitializer
{
    /// <summary>Gets or sets the used back-off policy.</summary>
    private ExponentialBackOffPolicy Policy { get; set; }

    /// <summary>Gets or sets the back-off handler creation function.</summary>
    private Func<BackOffHandler> CreateBackOff { get; set; }

    /// <summary>
    /// Constructs a new back-off initializer with the given policy and back-off handler create function.
    /// </summary>
    public ExponentialBackOffInitializer(ExponentialBackOffPolicy policy, Func<BackOffHandler> createBackOff)
    {
        Policy = policy;
        CreateBackOff = createBackOff;
    }

    public void Initialize(ConfigurableHttpClient httpClient)
    {
        BackOffHandler backOffHandler = CreateBackOff();
        if ((Policy & ExponentialBackOffPolicy.Exception) == ExponentialBackOffPolicy.Exception)
            httpClient.MessageHandler.AddExceptionHandler(backOffHandler);
        if ((Policy & ExponentialBackOffPolicy.UnsuccessfulResponse503) != ExponentialBackOffPolicy.UnsuccessfulResponse503)
            return;
        httpClient.MessageHandler.AddUnsuccessfulResponseHandler(backOffHandler);
    }
}
