using iCloud.Dav.Core.Enums;

namespace iCloud.Dav.Core.Services
{
    public interface IConfigurableHttpClientCredentialInitializer : IConfigurableHttpClientInitializer
    {
        string GetUriHomeSet(PrincipalHomeSet principal);
    }
}