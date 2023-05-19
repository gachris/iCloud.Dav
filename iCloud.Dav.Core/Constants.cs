namespace iCloud.Dav.Core
{
    /// <summary>
    /// A class containing constants used throughout the application.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// The value for the "Content-Type" header for "application/x-www-form-urlencoded".
        /// </summary>
        public const string APPLICATION_X_WWW_FORM_URLENCODED = "application/x-www-form-urlencoded";

        /// <summary>
        /// The value for the "Content-Type" header for "text/calendar".
        /// </summary>
        public const string TEXT_CALENDAR = "text/calendar";

        /// <summary>
        /// The value for the "Content-Type" header for "text/vcard".
        /// </summary>
        public const string TEXT_VCARD = "text/vcard";

        /// <summary>
        /// The value for the "Content-Type" header for "application/xml".
        /// </summary>
        public const string APPLICATION_XML = "application/xml";

        /// <summary>
        /// The HTTP GET method.
        /// </summary>
        public const string Get = "GET";

        /// <summary>
        /// The HTTP PUT method.
        /// </summary>
        public const string Put = "PUT";

        /// <summary>
        /// The HTTP DELETE method.
        /// </summary>
        public const string Delete = "DELETE";

        /// <summary>
        /// The HTTP PROPFIND method.
        /// </summary>
        public const string Propfind = "PROPFIND";

        /// <summary>
        /// The HTTP PROPPATCH method.
        /// </summary>
        public const string Proppatch = "PROPPATCH";

        /// <summary>
        /// The HTTP REPORT method.
        /// </summary>
        public const string Report = "REPORT";

        /// <summary>
        /// The HTTP MKCALENDAR method.
        /// </summary>
        public const string Mkcalendar = "MKCALENDAR";
    }
}