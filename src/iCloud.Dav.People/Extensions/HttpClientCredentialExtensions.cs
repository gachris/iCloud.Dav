using System;
using iCloud.Dav.Core;

namespace iCloud.Dav.People.Extensions;

internal static class HttpClientCredentialExtensions
{
    public static string GetAddressBookFullHref(this IConfigurableHttpClientCredentialInitializer clientCredentialInitializer, string resourceName, string contactGroupId)
    {
        var baseUri = clientCredentialInitializer.GetUri(PrincipalHomeSet.AddressBook);
        var relativeUri = string.Concat(resourceName, "/", contactGroupId, ".vcf");
        return new Uri(baseUri, relativeUri).AbsolutePath;
    }
}