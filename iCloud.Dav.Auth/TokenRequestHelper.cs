using iCloud.Dav.Core;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Serialization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth
{
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

            var peopleServer = await httpClient.GetCardDavServer(iCloudContactsBaseUrl, cancellationToken);
            var calendarServer = await httpClient.GetCalDavServer(iCloudCalendarBaseUrl, cancellationToken);
            var peoplePrincipal = await httpClient.GetPeoplePrincipal(peopleServer.Url, cancellationToken);
            var calendarPrincipal = await httpClient.GetCalendarPrincipal(calendarServer.Url, cancellationToken);

            return new Token(calendarServer, calendarPrincipal, peopleServer, peoplePrincipal, code, clock.Now);
        }

        private static async Task<DavServer> GetCalDavServer(this ConfigurableHttpClient httpClient, string url, CancellationToken cancellationToken)
        {
            var propFind = new WebDav.DataTypes.Cal.PropFind()
            {
                Prop = new WebDav.DataTypes.Cal.Prop()
                {
                    CurrentUserPrincipal = new WebDav.DataTypes.Cal.CurrentUserPrincipal()
                }
            };
            var multistatus = await httpClient.SendCalDavPropFindRequest(url, propFind, cancellationToken);
            var multistatusResponse = multistatus.Responses.FirstOrDefault().ThrowIfNull(nameof(WebDav.DataTypes.Cal.MultiStatus));
            var id = multistatusResponse.PropStat[0].Prop.CurrentUserPrincipal.Href.Value.Split('/')[1];
            return new DavServer(id, string.Concat(url, multistatusResponse.PropStat[0].Prop.CurrentUserPrincipal.Href.Value));
        }

        private static async Task<DavServer> GetCardDavServer(this ConfigurableHttpClient httpClient, string url, CancellationToken cancellationToken)
        {
            var propFind = new WebDav.DataTypes.Card.PropFind()
            {
                Prop = new WebDav.DataTypes.Card.Prop()
                {
                    CurrentUserPrincipal = new WebDav.DataTypes.Card.CurrentUserPrincipal()
                }
            };
            var multistatus = await httpClient.SendCardDavPropFindRequest(url, propFind, cancellationToken);
            var multistatusResponse = multistatus.Responses.FirstOrDefault().ThrowIfNull(nameof(WebDav.DataTypes.Card.MultiStatus));
            var id = multistatusResponse.PropStat[0].Prop.CurrentUserPrincipal.Href.Value.Split('/')[1];
            return new DavServer(id, string.Concat(url, multistatusResponse.PropStat[0].Prop.CurrentUserPrincipal.Href.Value));
        }

        private static async Task<CalendarPrincipal> GetCalendarPrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var propFind = new WebDav.DataTypes.Cal.PropFind()
            {
                Prop = new WebDav.DataTypes.Cal.Prop()
                {
                    CurrentUserPrincipal = new WebDav.DataTypes.Cal.CurrentUserPrincipal(),
                    DisplayName = new WebDav.DataTypes.Cal.DisplayName(),
                    CalendarHomeSet = new WebDav.DataTypes.Cal.CalendarHomeSet(),
                    CalendarUserAddressSet = new WebDav.DataTypes.Cal.CalendarUserAddressSet()
                }
            };
            var multistatus = await httpClient.SendCalDavPropFindRequest(requestUri, propFind, cancellationToken);
            var multistatusResponse = multistatus.Responses.FirstOrDefault().ThrowIfNull(nameof(WebDav.DataTypes.Cal.MultiStatus));
            var calendarUserAddressSets = multistatusResponse.PropStat[0].Prop.CalendarUserAddressSet.Values.Select(url => new CalendarUserAddressSet(url.Value, url.Preferred));
            return new CalendarPrincipal(multistatusResponse.PropStat[0].Prop.CurrentUserPrincipal.Href.Value, multistatusResponse.PropStat[0].Prop.CalendarHomeSet.Href.Value, multistatusResponse.PropStat[0].Prop.DisplayName?.Value, calendarUserAddressSets);
        }

        private static async Task<PeoplePrincipal> GetPeoplePrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var propFind = new WebDav.DataTypes.Card.PropFind()
            {
                Prop = new WebDav.DataTypes.Card.Prop()
                {
                    CurrentUserPrincipal = new WebDav.DataTypes.Card.CurrentUserPrincipal(),
                    DisplayName = new WebDav.DataTypes.Card.DisplayName(),
                    AddressbookHomeSet = new WebDav.DataTypes.Card.AddressbookHomeSet()
                }
            };
            var multistatus = await httpClient.SendCardDavPropFindRequest(requestUri, propFind, cancellationToken);
            var multistatusResponse = multistatus.Responses.FirstOrDefault().ThrowIfNull(nameof(WebDav.DataTypes.Card.MultiStatus));
            return new PeoplePrincipal(multistatusResponse.PropStat[0].Prop.CurrentUserPrincipal.Href.Value, multistatusResponse.PropStat[0].Prop.AddressbookHomeSet.Href.Value, multistatusResponse.PropStat[0].Prop.DisplayName?.Value);
        }

        private static async Task<WebDav.DataTypes.Cal.MultiStatus> SendCalDavPropFindRequest(this ConfigurableHttpClient httpClient, string requestUri, object request, CancellationToken cancellationToken)
        {
            var content = XmlObjectSerializer.Instance.Serialize(request);
            var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(Constants.Propfind), requestUri)
            {
                Content = new StringContent(content),
            }, cancellationToken).ConfigureAwait(false);

            var input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return !response.IsSuccessStatusCode
                ? throw new TokenException(new ErrorResponse(response.ReasonPhrase, response.StatusCode, requestUri, input))
                : XmlObjectSerializer.Instance.Deserialize<WebDav.DataTypes.Cal.MultiStatus>(input);
        }

        private static async Task<WebDav.DataTypes.Card.MultiStatus> SendCardDavPropFindRequest(this ConfigurableHttpClient httpClient, string requestUri, object request, CancellationToken cancellationToken)
        {
            var content = XmlObjectSerializer.Instance.Serialize(request);
            var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(Constants.Propfind), requestUri)
            {
                Content = new StringContent(content),
            }, cancellationToken).ConfigureAwait(false);

            var input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return !response.IsSuccessStatusCode
                ? throw new TokenException(new ErrorResponse(response.ReasonPhrase, response.StatusCode, requestUri, input))
                : XmlObjectSerializer.Instance.Deserialize<WebDav.DataTypes.Card.MultiStatus>(input);
        }
    }
}