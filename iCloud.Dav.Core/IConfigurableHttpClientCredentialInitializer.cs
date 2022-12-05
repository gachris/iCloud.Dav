using System;

namespace iCloud.Dav.Core
{
    public interface IConfigurableHttpClientCredentialInitializer : IConfigurableHttpClientInitializer
    {
        Uri GetUri(PrincipalHomeSet principal);
    }
}