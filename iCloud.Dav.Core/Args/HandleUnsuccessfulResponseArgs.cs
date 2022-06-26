using iCloud.Dav.Core.Services;
using System.Net.Http;
using System.Threading;

namespace iCloud.Dav.Core.Args
{
    /// <summary>Argument class to <see cref="IHttpUnsuccessfulResponseHandler.HandleResponseAsync(HandleUnsuccessfulResponseArgs)" />.</summary>
    public class HandleUnsuccessfulResponseArgs
    {
        /// <summary>Gets or sets the sent request.</summary>
        public HttpRequestMessage Request { get; set; }

        /// <summary>Gets or sets the abnormal response.</summary>
        public HttpResponseMessage Response { get; set; }

        /// <summary>Gets or sets the total number of tries to send the request.</summary>
        public int TotalTries { get; set; }

        /// <summary>Gets or sets the current failed try.</summary>
        public int CurrentFailedTry { get; set; }

        /// <summary>Gets an indication whether a retry will occur if the handler returns <c>true</c>.</summary>
        public bool SupportsRetry => TotalTries - CurrentFailedTry > 0;

        /// <summary>Gets or sets the request's cancellation token.</summary>
        public CancellationToken CancellationToken { get; set; }
    }
}
