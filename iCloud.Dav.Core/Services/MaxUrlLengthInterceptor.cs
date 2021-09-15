using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Core.Services
{
    /// <summary>
    /// Intercepts HTTP GET requests with a URLs longer than a specified maximum number of characters.
    /// The interceptor will change such requests as follows:
    /// <list type="bullet">
    /// <item>The request's method will be changed to POST</item>
    /// <item>A <c>X-HTTP-Method-Override</c> header will be added with the value <c>GET</c></item>
    /// <item>Any query parameters from the URI will be moved into the body of the request.</item>
    /// <item>If query parameters are moved, the content type is set to <c>application/x-www-form-urlencoded</c></item>
    /// </list>
    /// </summary>
    public class MaxUrlLengthInterceptor : IHttpExecuteInterceptor
    {
        private readonly uint maxUrlLength;

        /// <summary>Constructs a new Max URL length interceptor with the given max length.</summary>
        public MaxUrlLengthInterceptor(uint maxUrlLength)
        {
            this.maxUrlLength = maxUrlLength;
        }

        public Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method != HttpMethod.Get || request.RequestUri.AbsoluteUri.Length <= maxUrlLength)
            {
#if net40
                return TaskEx.Delay(0);
#endif
#if others_frameworks
                return Task.Delay(0);
#endif
            }
            request.Method = HttpMethod.Post;
            string query = request.RequestUri.Query;
            if (!string.IsNullOrEmpty(query))
            {
                request.Content = new StringContent(query.Substring(1));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                string str = request.RequestUri.ToString();
                request.RequestUri = (new Uri(str.Remove(str.IndexOf("?"))));
            }
            request.Headers.Add("X-HTTP-Method-Override", "GET");
#if net40
            return TaskEx.Delay(0);
#endif
#if others_frameworks
            return Task.Delay(0);
#endif
        }
    }
}
