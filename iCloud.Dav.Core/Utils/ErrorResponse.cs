using System.Net;

namespace iCloud.Dav.Core.Utils
{
    /// <summary>Model for a unsuccessful access response as specified in</summary>
    public class ErrorResponse
    {
        /// <summary>Gets or sets reason phrase/</summary>
        public string ReasonPhrase { get; set; }

        /// <summary>Gets or sets a HttpStatusCode</summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>Gets or sets the URI identifying a human-readable web page with provides information about the error.</summary>
        public string ErrorUri { get; set; }

        /// <summary>Gets or sets error description.</summary>
        public string ErrorDescription { get; set; }

        public override string ToString()
        {
            return string.Format("ReasonPhrase:\"{0}\", HttpStatusCode:\"{1}\", Uri:\"{2}\", ErrorDescription:\"{3}\"", ReasonPhrase, HttpStatusCode, ErrorUri, ErrorDescription);
        }

        /// <summary>Constructs a new empty error response.</summary>
        public ErrorResponse()
        {
        }
    }
}
