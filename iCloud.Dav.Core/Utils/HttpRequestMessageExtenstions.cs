using iCloud.Dav.Core.Services;
using System;
using System.Net.Http;
using System.Text;

namespace iCloud.Dav.Core.Utils
{
    /// <summary>Extension methods to <see cref="T:System.Net.Http.HttpRequestMessage" />.</summary>
    internal static class HttpRequestMessageExtenstions
    {
        /// <summary>
        /// Sets the content of the request by the given body.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="service">The service.</param>
        /// <param name="body">The body of the future request. If <c>null</c> do nothing.</param>
        /// <param name="contentType">The content type of the future request. If <c>null</c> do nothing.</param>
        internal static void SetRequestSerailizedContent(this HttpRequestMessage request, IClientService service, object body, string contentType = null)
        {
            if (body == null)
                return;
            string mediaType = contentType;
            if (String.IsNullOrEmpty(mediaType))
                mediaType = "application/" + service.Serializer.Format;
            string content = service.SerializeObject(body);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);
            request.Content = httpContent;
        }

        internal static void SetHttpRequestDepth(this HttpRequestMessage request, string Depth)
        {
            if (!string.IsNullOrEmpty(Depth))
            {
                request.Headers.Add(nameof(Depth), Depth);
            }
        }
    }
}
