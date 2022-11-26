using System.Net;

namespace iCloud.Dav.Core
{
    /// <summary>Model for a unsuccessful access response as specified in</summary>
    public class ErrorResponse
    {
        /// <summary>Gets reason phrase/</summary>
        public string ReasonPhrase { get; }

        /// <summary>Gets HttpStatusCode</summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>Gets the URI identifying a human-readable web page with provides information about the error.</summary>
        public string ErrorUri { get; }

        /// <summary>Gets error description.</summary>
        public string ErrorDescription { get; }

        public override string ToString() => string.Format("ReasonPhrase:\"{0}\", HttpStatusCode:\"{1}\", Uri:\"{2}\", ErrorDescription:\"{3}\"",
                                 ReasonPhrase,
                                 HttpStatusCode,
                                 ErrorUri,
                                 ErrorDescription);

        /// <summary>Constructs a new empty error response.</summary>
        public ErrorResponse(string reasonPhrase, HttpStatusCode httpStatusCode, string errorUri, string errorDescription)
        {
            ReasonPhrase = reasonPhrase;
            HttpStatusCode = httpStatusCode;
            ErrorUri = errorUri;
            ErrorDescription = errorDescription;
        }
    }
}