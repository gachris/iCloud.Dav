using System.Net.Http;
using System.Net.Http.Headers;

namespace iCloud.Dav.Auth
{
    /// <summary>Helper for accessing protected resources using Basic Authentication.</summary>
    public class BasicAuthentication
    {
        /// <summary>Method for accessing protected resources using the Authorization header.</summary>
        public class AuthorizationHeaderAccessMethod : IAccessMethod
        {
            private const string Schema = "Basic";

            /// <summary>
            /// Intercepts a HTTP request right before the HTTP request executes by providing the access token.
            /// </summary>
            public void Intercept(HttpRequestMessage request, string accessToken) => request.Headers.Authorization = new AuthenticationHeaderValue("Basic", accessToken);

            /// <summary>
            /// Retrieves the original access token in the HTTP request, as provided in the <see cref="IAccessMethod.Intercept(HttpRequestMessage,System.String)" />
            /// method.
            /// </summary>
            public string GetAccessToken(HttpRequestMessage request)
            {
                if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Basic")
                    return request.Headers.Authorization.Parameter;
                return null;
            }
        }
    }
}