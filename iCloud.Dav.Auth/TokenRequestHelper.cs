using iCloud.Dav.Auth.CardDav.Types;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Serialization;
using iCloud.Dav.Core.Utils;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth;

internal static class TokenRequestHelper
{
    private const string iCloudContactsBaseUrl = "https://contacts.icloud.com";
    private const string iCloudCalendarBaseUrl = "https://caldav.icloud.com";

    /// <summary>
    /// Executes the token request in order to receive a
    /// <see cref="Token" />. In case the token server returns an
    /// error, a <see cref="TokenException" /> is thrown.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to create an HTTP request.</param>
    /// <param name="code">Authorization Basic token</param>
    /// <param name="cancellationToken">Cancellation token to cancel operation.</param>
    /// <param name="clock">
    /// The clock which is used to set the
    /// <see cref="Token.Issued" /> property.
    /// </param>
    /// <returns>Token response.</returns>
    public static async Task<Token> ExecuteAsync(this ConfigurableHttpClient httpClient, string code, IClock clock, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {code}");
        httpClient.DefaultRequestHeaders.Add("Depth", "0");

        var peopleServer = await httpClient.GetServer(iCloudContactsBaseUrl, cancellationToken);
        var calendarServer = await httpClient.GetServer(iCloudCalendarBaseUrl, cancellationToken);
        var peoplePrincipal = await httpClient.GetPeoplePrincipal(peopleServer.Url, cancellationToken);
        var calendarPrincipal = await httpClient.GetCalendarPrincipal(calendarServer.Url, cancellationToken);

        return new Token(calendarServer, calendarPrincipal, peopleServer, peoplePrincipal, code, clock.Now);
    }

    private static async Task<DavServer> GetServer(this ConfigurableHttpClient httpClient, string url, CancellationToken cancellationToken)
    {
        var userPrincipalRequest = new PropFind() { CurrentUserPrincipal = true };
        var multistatus = await httpClient.SendPropFindRequest(url, userPrincipalRequest, cancellationToken);
        var multistatusResponse = multistatus.Responses.FirstOrDefault().ThrowIfNull(nameof(MultiStatus));
        var id = multistatusResponse.CurrentUserPrincipal.Split('/')[1];
        return new DavServer(id, string.Concat(url, multistatusResponse.CurrentUserPrincipal));
    }

    private static async Task<CalendarPrincipal> GetCalendarPrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
    {
        var principalRequest = new PropFind() { CurrentUserPrincipal = true, CalendarHomeSet = true, CalendarUserAddressSet = true, DisplayName = true };
        var multistatus = await httpClient.SendPropFindRequest(requestUri, principalRequest, cancellationToken);
        var multistatusResponse = multistatus.Responses.FirstOrDefault().ThrowIfNull(nameof(MultiStatus));
        var calendarUserAddressSets = multistatusResponse.CalendarUserAddressSet.Select(url => new CalendarUserAddressSet(url.Value, url.Preferred));
        return new CalendarPrincipal(multistatusResponse.CurrentUserPrincipal, multistatusResponse.CalendarHomeSet!, multistatusResponse.DisplayName!, calendarUserAddressSets);
    }

    private static async Task<PeoplePrincipal> GetPeoplePrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
    {
        var principalRequest = new PropFind() { CurrentUserPrincipal = true, AddressBookHomeSet = true, DisplayName = true };
        var multistatus = await httpClient.SendPropFindRequest(requestUri, principalRequest, cancellationToken);
        var multistatusResponse = multistatus.Responses.FirstOrDefault().ThrowIfNull(nameof(MultiStatus));
        return new PeoplePrincipal(multistatusResponse.CurrentUserPrincipal, multistatusResponse.AddressBookHomeSet!, multistatusResponse.DisplayName!);
    }

    private static async Task<MultiStatus> SendPropFindRequest(this ConfigurableHttpClient httpClient, string requestUri, object request, CancellationToken cancellationToken)
    {
        var content = XmlObjectSerializer.Instance.Serialize(request);
        var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(Constants.Propfind), requestUri)
        {
            Content = new StringContent(content),
        }, cancellationToken).ConfigureAwait(false);

        var input = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new TokenException(new ErrorResponse(response.ReasonPhrase, response.StatusCode, requestUri, input));
        }

        return XmlObjectSerializer.Instance.Deserialize<MultiStatus>(input);
    }
}
