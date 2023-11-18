using iCloud.Dav.Core;

namespace iCloud.Dav.Auth;

/// <summary>
/// Allows direct retrieval of access tokens to authenticate requests.
/// This is necessary for workflows where you don't want to use
/// <see cref="BaseClientService" /> to access the API.
/// (e.g. gRPC that implemenents the entire HTTP2 stack internally).
/// </summary>
public interface ITokenAccess
{
    /// <summary>
    /// Gets an access token to authorize a request.
    /// </summary>
    /// <returns>The access token.</returns>
    string GetAccessTokenForRequestAsync();
}