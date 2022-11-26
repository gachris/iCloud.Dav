using System.Net.Http;

namespace iCloud.Dav.Auth
{
    /// <summary>Method of presenting the access token to the resource server.</summary>
    public interface IAccessMethod
    {
        /// <summary>
        /// Intercepts a HTTP request right before the HTTP request executes by providing the access token.
        /// </summary>
        void Intercept(HttpRequestMessage request, string accessToken);

        /// <summary>
        /// Retrieves the original access token in the HTTP request, as provided in the <see cref="IAccessMethod.Intercept(HttpRequestMessage,System.String)" />
        /// method.
        /// </summary>
        string GetAccessToken(HttpRequestMessage request);
    }
}