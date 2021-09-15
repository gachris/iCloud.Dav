using iCloud.Dav.Core.Services;

namespace iCloud.Dav.Auth
{
    /// <summary>
    /// The main interface to represent credential in the client library.
    /// Service account, User account and Compute credential inherit from this interface
    /// to provide access token functionality. In addition this interface inherits from
    /// <see cref="T:iCloud.dav.Auth.IConfigurableHttpClientCredentialInitializer" /> to be able to hook to http requests.
    /// </summary>
    public interface ICredential : IConfigurableHttpClientCredentialInitializer, ITokenAccess
    {
    }
}
