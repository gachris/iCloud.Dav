using System;
using System.Net;
using System.Net.Http;

namespace iCloud.Dav.Core.Utils
{
    /// <summary>
    /// Extension methods to <see cref="HttpRequestMessage" /> and
    /// <see cref="HttpResponseMessage" />.
    /// </summary>
    internal static class HttpExtenstions
    {
        /// <summary>Returns <c>true</c> if the response contains one of the redirect status codes.</summary>
        internal static bool IsRedirectStatusCode(this HttpResponseMessage message)
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

        /// <summary>A ICloud.Api utility method for setting an empty HTTP content.</summary>
        public static HttpContent SetEmptyContent(this HttpRequestMessage request)
        {
            request.Content = new ByteArrayContent(Array.Empty<byte>());
            request.Content.Headers.ContentLength = new long?(0L);
            return request.Content;
        }
    }
}