using System;
using System.Net;
using System.Net.Http;

namespace iCloud.Dav.Core.Extensions
{
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
            switch (message.StatusCode)
            {
                case HttpStatusCode.Moved:
                case HttpStatusCode.Found:
                case HttpStatusCode.RedirectMethod:
                case HttpStatusCode.RedirectKeepVerb:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// A utility method for setting an empty HTTP content.
        /// </summary>
        /// <param name="request">The HTTP request message for which to set an empty content.</param>
        /// <returns>The empty HTTP content that was set.</returns>
        public static HttpContent SetEmptyContent(this HttpRequestMessage request)
        {
            request.Content = new ByteArrayContent(Array.Empty<byte>());
            request.Content.Headers.ContentLength = new long?(0L);
            return request.Content;
        }
    }
}