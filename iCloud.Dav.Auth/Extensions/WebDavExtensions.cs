using System;
using System.Net;

namespace iCloud.Dav.Auth.Extensions
{
    internal static class WebDavExtensions
    {
        public static HttpStatusCode? ToHttpStatusCode(this string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return null;

            var statusParts = status.Split(' ');
            var statusCodeString = statusParts.Length > 0 ? (int?)Convert.ToInt16(statusParts[1]) : null;
            return (HttpStatusCode?)statusCodeString;
        }
    }
}