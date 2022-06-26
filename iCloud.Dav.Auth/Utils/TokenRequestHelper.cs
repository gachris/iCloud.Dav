using iCloud.Dav.Auth.CardDav.Types;
using iCloud.Dav.Core.Serializer;
using iCloud.Dav.Core.Services;
using iCloud.Dav.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth.Utils
{
    internal static class TokenRequestHelper
    {
        private const string Contacts_icloud = "https://contacts.icloud.com";
        private const string Caldav_icloud = "https://caldav.icloud.com";

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
            var token = new Token() { AccessToken = code, Issued = clock.Now };

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {code}");
            httpClient.DefaultRequestHeaders.Add("Depth", "0");

            token.PeopleServer = await httpClient.GetServer(Contacts_icloud, Contacts_icloud, cancellationToken);
            token.CalendarServer = await httpClient.GetServer(Caldav_icloud, Caldav_icloud, cancellationToken);
            token.PeoplePrincipal = await httpClient.GetPeoplePrincipal(token.PeopleServer.Url, cancellationToken);
            token.CalendarPrincipal = await httpClient.GetCalendarPrincipal(token.CalendarServer.Url, cancellationToken);

            return token;
        }

        private static async Task<DavServer> GetServer(this ConfigurableHttpClient httpClient, string serverUri, string requestUri, CancellationToken cancellationToken)
        {
            var userPrincipalRequest = new PropFind() { CurrentUserPrincipal = true };
            var multistatus = await httpClient.SendPropFindRequest(requestUri, userPrincipalRequest, cancellationToken);
            var multistatusResponse = multistatus.Responses.FirstOrDefault();
            var id = multistatusResponse.CurrentUserPrincipal.Split(new char[] { '/' })[1];
            var url = string.Concat(serverUri, multistatusResponse.CurrentUserPrincipal);
            return new DavServer(id, url);
        }

        private static async Task<CalendarPrincipal> GetCalendarPrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var principalRequest = new PropFind() { CurrentUserPrincipal = true, CalendarHomeSet = true, CalendarUserAddressSet = true, DisplayName = true };
            var multistatus = await httpClient.SendPropFindRequest(requestUri, principalRequest, cancellationToken);
            var multistatusResponse = multistatus.Responses.FirstOrDefault();

            var calendarUserAddressSets = multistatusResponse.CalendarUserAddressSet.
                Select(url => new CalendarUserAddressSet { Url = url.Value, Preferred = url.Preferred });

            return new CalendarPrincipal
            {
                CalendarUserAddressSet = new List<CalendarUserAddressSet>(calendarUserAddressSets),
                CalendarHomeSet = multistatusResponse.CalendarHomeSet,
                DisplayName = multistatusResponse.DisplayName,
                CurrentUserPrincipal = multistatusResponse.CurrentUserPrincipal
            };
        }

        private async static Task<PeoplePrincipal> GetPeoplePrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var principalRequest = new PropFind() { CurrentUserPrincipal = true, AddressBookHomeSet = true, DisplayName = true };
            var multistatus = await httpClient.SendPropFindRequest(requestUri, principalRequest, cancellationToken);
            var multistatusResponse = multistatus.Responses.FirstOrDefault();

            return new PeoplePrincipal
            {
                AddressBookHomeSet = multistatusResponse.AddressBookHomeSet,
                DisplayName = multistatusResponse.DisplayName,
                CurrentUserPrincipal = multistatusResponse.CurrentUserPrincipal
            };
        }

        private async static Task<Multistatus> SendPropFindRequest(this ConfigurableHttpClient httpClient, string requestUri, object request, CancellationToken cancellationToken)
        {
            var content = XmlObjectSerializer.Instance.Serialize(request);
            var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.Propfind), requestUri)
            {
                Content = new StringContent(content),
            }, cancellationToken).ConfigureAwait(false);

            var input = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new TokenException(new ErrorResponse()
                {
                    ReasonPhrase = response.ReasonPhrase,
                    HttpStatusCode = response.StatusCode,
                    ErrorUri = requestUri,
                    ErrorDescription = input
                });
            }

            return XmlObjectSerializer.Instance.Deserialize<Multistatus>(input);
        }
    }
}
