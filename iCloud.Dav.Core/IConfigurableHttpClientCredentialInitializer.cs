using System;

namespace iCloud.Dav.Core;

/// <summary>
/// Interface for a configurable HTTP client credential initializer that can be used to set custom
/// properties on <see cref="ConfigurableHttpClient" /> and <see cref="ConfigurableMessageHandler" />.
/// </summary>
public interface IConfigurableHttpClientCredentialInitializer : IConfigurableHttpClientInitializer
{
    /// <summary>
    /// Gets the URI of the principal home set.
    /// </summary>
    /// <param name="principal">The principal home set.</param>
    /// <returns>The URI of the principal home set.</returns>
    Uri GetUri(PrincipalHomeSet principal);
}