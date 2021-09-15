using iCloud.Dav.Auth.Types;
using iCloud.Dav.Core.Serializer;
using iCloud.Dav.Core.Services;
using iCloud.Dav.Core.Utils;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Auth.Utils
{
    internal static class TokenRequestHelper
    {
        private const string _peopleUrl = "https://contacts.icloud.com";
        private const string _calendarUrl = "https://caldav.icloud.com";

        /// <summary>
        /// Executes the token request in order to receive a
        /// <see cref="T:iCloud.dav.Auth.Token" />. In case the token server returns an
        /// error, a <see cref="T:iCloud.dav.Auth.Response.TokenResponseException" /> is thrown.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to create an HTTP request.</param>
        /// <param name="code">Authorization Basic token</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        /// <param name="clock">
        /// The clock which is used to set the
        /// <see cref="P:iCloud.dav.Auth.Token.Issued" /> property.
        /// </param>
        /// <returns>Token response.</returns>
        public static async Task<Token> ExecuteAsync(this ConfigurableHttpClient httpClient, string code, CancellationToken taskCancellationToken, IClock clock)
        {
            Token token = new Token() { AccessToken = code, Issued = clock.Now };

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {code}");
            httpClient.DefaultRequestHeaders.Add("Depth", "0");

            token.PeopleServer = await httpClient.GetPeopleServer(_peopleUrl, taskCancellationToken);
            token.PeoplePrincipal = await httpClient.GetPeoplePrincipal(token.PeopleServer.Url, taskCancellationToken);
            token.CalendarServer = await httpClient.GetCalendarServer(_calendarUrl, taskCancellationToken);
            token.CalendarPrincipal = await httpClient.GetCalendarPrincipal(token.CalendarServer.Url, taskCancellationToken);

            return token;
        }

        private static async Task<DavServer> GetCalendarServer(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var propfind = new Propfind<Prop>() { Prop = new Prop() { CurrentUserPrincipal = CurrentUserPrincipal.Default } };
            var content = XmlSerializer.Instance.Serialize(propfind);

            HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), requestUri)
            {
                Content = new StringContent(content),
            }, cancellationToken).ConfigureAwait(false);

            string input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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

            var multistatus = XmlSerializer.Instance.Deserialize<Multistatus<Prop>>(input);

            string id = multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value.Split(new char[] { '/' })[1];
            string url = _calendarUrl + multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value;

            DavServer server = new DavServer(id, url);
            return server;
        }

        private static async Task<DavServer> GetPeopleServer(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var propfind = new Propfind<Prop>() { Prop = new Prop() { CurrentUserPrincipal = CurrentUserPrincipal.Default } };
            var content = XmlSerializer.Instance.Serialize(propfind);

            HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), requestUri)
            {
                Content = new StringContent(content),
            }, cancellationToken).ConfigureAwait(false);

            string input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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

            Multistatus<Prop> multistatus = XmlSerializer.Instance.Deserialize<Multistatus<Prop>>(input);

            string id = multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value.Split(new char[] { '/' })[1];
            string url = _peopleUrl + multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value;

            DavServer server = new DavServer(id, url);
            return server;
        }

        private static async Task<CalendarPrincipal> GetCalendarPrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            Propfind<Prop> principalRequest = new Propfind<Prop>()
            {
                Prop = new Prop()
                {
                    CalendarUserAddressSet = Types.CalendarUserAddressSet.Default,
                    CalendarHomeSet = CalendarHomeSet.Default,
                    CurrentUserPrincipal = CurrentUserPrincipal.Default,
                    DisplayName = DisplayName.Default
                }
            };

            string content = XmlSerializer.Instance.Serialize(principalRequest);

            HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), requestUri)
            {
                Content = new StringContent(content),
            }, cancellationToken).ConfigureAwait(false);

            string input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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

            Multistatus<Prop> multistatus = XmlSerializer.Instance.Deserialize<Multistatus<Prop>>(input);

            var calendarUserAddressSets = multistatus.Responses.FirstOrDefault().
                Propstat.Prop.CalendarUserAddressSet.Href.Select(new System.Func<Url, CalendarUserAddressSet>((url) =>
            {
                CalendarUserAddressSet calendarUserAddressSet = new CalendarUserAddressSet();
                calendarUserAddressSet.Url = url.Value;
                calendarUserAddressSet.Preferred = url.Preferred;
                return calendarUserAddressSet;
            }));

            CalendarPrincipal calendarPrincipal = new CalendarPrincipal();
            calendarPrincipal.CalendarUserAddressSet = new CalendarUserAddressSetList(calendarUserAddressSets);
            calendarPrincipal.CalendarHomeSet = multistatus.Responses.FirstOrDefault().Propstat.Prop.CalendarHomeSet.Url.Value;
            calendarPrincipal.DisplayName = multistatus.Responses.FirstOrDefault().Propstat.Prop.DisplayName.Value;
            calendarPrincipal.CurrentUserPrincipal = multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value;
            return calendarPrincipal;
        }

        private async static Task<PeoplePrincipal> GetPeoplePrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            Propfind<Prop> principalRequest = new Propfind<Prop>()
            {
                Prop = new Prop()
                {
                    AddressBookHomeSet = AddressBookHomeSet.Default,
                    CurrentUserPrincipal = CurrentUserPrincipal.Default,
                    DisplayName = DisplayName.Default
                }
            };
            string content = XmlSerializer.Instance.Serialize(principalRequest);

            HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), requestUri)
            {
                Content = new StringContent(content),
            }, cancellationToken).ConfigureAwait(false);

            string input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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

            Multistatus<Prop> multistatus = XmlSerializer.Instance.Deserialize<Multistatus<Prop>>(input);
            PeoplePrincipal peoplePrincipal = new PeoplePrincipal();
            peoplePrincipal.AddressBookHomeSet = multistatus.Responses.FirstOrDefault().Propstat.Prop.AddressBookHomeSet.Url.Value;
            peoplePrincipal.DisplayName = multistatus.Responses.FirstOrDefault().Propstat.Prop.DisplayName.Value;
            peoplePrincipal.CurrentUserPrincipal = multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value;
            return peoplePrincipal;
        }
    }
}
