using System.Net.Http;
using System.Text;

namespace iCloud.Dav.Core.Extensions;

/// <summary>
/// Extension methods to <see cref="HttpRequestMessage"/>.
/// </summary>
internal static class HttpRequestMessageExtensions
{
    /// <summary>
    /// Sets the content of the request by the given body.
    /// </summary>
    /// <param name="request">The HTTP request message to which to set the content.</param>
    /// <param name="service">The client service associated with the request.</param>
    /// <param name="body">The body of the request to set as the content. If <c>null</c>, the content is not set.</param>
    /// <param name="contentType">The content type of the request. If <c>null</c>, the default content type for the client service serializer is used.</param>
    public static void SetRequestSerializedContent(this HttpRequestMessage request, IClientService service, object body, string contentType = null)
    {
        if (body == null)
        {
            return;
        }
        var mediaType = contentType;
        if (string.IsNullOrEmpty(mediaType))
        {
            mediaType = "application/" + service.Serializer.Format;
        }
        var content = service.SerializeObject(body);
        var httpContent = new StringContent(content, Encoding.UTF8, mediaType);
        request.Content = httpContent;
    }

    /// <summary>
    /// Sets the depth header of the request.
    /// </summary>
    /// <param name="request">The HTTP request message to which to set the depth header.</param>
    /// <param name="depth">The value of the depth header. If <c>null</c> or empty, the depth header is not set.</param>
    public static void SetHttpRequestDepth(this HttpRequestMessage request, string depth)
    {
        if (string.IsNullOrEmpty(depth))
        {
            return;
        }
        request.Headers.Add(nameof(depth), depth);
    }
}