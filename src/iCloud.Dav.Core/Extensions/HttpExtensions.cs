using System.Net;

namespace iCloud.Dav.Core.Extensions;

/// <summary>
/// Extension methods to <see cref="HttpRequestMessage"/> and <see cref="HttpResponseMessage"/>.
/// </summary>
internal static class HttpExtensions
{
    /// <summary>
    /// Returns <c>true</c> if the response contains one of the redirect status codes.
    /// </summary>
    /// <param name="message">The HTTP response message to check for a redirect status code.</param>
    /// <returns><c>true</c> if the response contains one of the redirect status codes; otherwise, <c>false</c>.</returns>
    public static bool IsRedirectStatusCode(this HttpResponseMessage message)
    {
        return message.StatusCode switch
        {
            HttpStatusCode.Moved or HttpStatusCode.Found or HttpStatusCode.RedirectMethod or HttpStatusCode.RedirectKeepVerb => true,
            _ => false,
        };
    }
}