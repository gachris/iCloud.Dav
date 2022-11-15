namespace iCloud.Dav.Core;

public interface IConfigurableHttpClientCredentialInitializer : IConfigurableHttpClientInitializer
{
    string GetUri(PrincipalHomeSet principal);
}