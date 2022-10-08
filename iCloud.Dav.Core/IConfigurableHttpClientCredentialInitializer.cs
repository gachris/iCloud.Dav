namespace iCloud.Dav.Core;

public interface IConfigurableHttpClientCredentialInitializer : IConfigurableHttpClientInitializer
{
    string GetUriHomeSet(PrincipalHomeSet principal);
}