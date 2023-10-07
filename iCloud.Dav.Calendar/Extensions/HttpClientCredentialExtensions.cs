using iCloud.Dav.Core;
using System;

namespace iCloud.Dav.Calendar.Extensions;

internal static class HttpClientCredentialExtensions
{
    public static string GetCalendarFullHref(this IConfigurableHttpClientCredentialInitializer clientCredentialInitializer, string calendarId, string reminderId)
    {
        var baseUri = clientCredentialInitializer.GetUri(PrincipalHomeSet.Calendar);
        var relativeUri = string.Concat(calendarId, "/", reminderId, ".ics");
        return new Uri(baseUri, relativeUri).AbsolutePath;
    }
}