using iCloud.Dav.Auth.Types;
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
        private const string _peopleUrl = "https://contacts.icloud.com";
        private const string _calendarUrl = "https://caldav.icloud.com";

        /// <summary>
        /// Executes the token request in order to receive a
        /// <see cref="T:iCloud.dav.Auth.Token" />. In case the token server returns an
        /// error, a <see cref="T:iCloud.dav.Auth.Response.TokenResponseException" /> is thrown.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to create an HTTP request.</param>
        /// <param name="code">Authorization Basic token</param>
        /// <param name="cancellationToken">Cancellation token to cancel operation.</param>
        /// <param name="clock">
        /// The clock which is used to set the
        /// <see cref="P:iCloud.dav.Auth.Token.Issued" /> property.
        /// </param>
        /// <returns>Token response.</returns>
        public static async Task<Token> ExecuteAsync(this ConfigurableHttpClient httpClient, string code, IClock clock, CancellationToken cancellationToken)
        {
            var token = new Token() { AccessToken = code, Issued = clock.Now };

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {code}");
            httpClient.DefaultRequestHeaders.Add("Depth", "0");

            token.PeopleServer = await httpClient.GetPeopleServer(_peopleUrl, cancellationToken);
            token.PeoplePrincipal = await httpClient.GetPeoplePrincipal(token.PeopleServer.Url, cancellationToken);
            token.CalendarServer = await httpClient.GetCalendarServer(_calendarUrl, cancellationToken);
            token.CalendarPrincipal = await httpClient.GetCalendarPrincipal(token.CalendarServer.Url, cancellationToken);

            return token;
        }

        private static async Task<DavServer> GetCalendarServer(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var propfind = new Propfind() { Prop = new Prop() { CurrentUserPrincipal = CurrentUserPrincipal.Default } };
            var content = XmlSerializer.Instance.Serialize(propfind);

            var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), requestUri)
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

            var multistatus = XmlSerializer.Instance.Deserialize<Multistatus>(input);

            var id = multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value.Split(new char[] { '/' })[1];
            var url = _calendarUrl + multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value;

            var server = new DavServer(id, url);
            return server;
        }

        private static async Task<DavServer> GetPeopleServer(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var propfind = new Propfind() { Prop = new Prop() { CurrentUserPrincipal = CurrentUserPrincipal.Default } };
            var content = XmlSerializer.Instance.Serialize(propfind);

            var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), requestUri)
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

            var multistatus = XmlSerializer.Instance.Deserialize<Multistatus>(input);

            var id = multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value.Split(new char[] { '/' })[1];
            var url = _peopleUrl + multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value;

            var server = new DavServer(id, url);
            return server;
        }

        private static async Task<CalendarPrincipal> GetCalendarPrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var principalRequest = new Propfind()
            {
                Prop = new Prop()
                {
                    CalendarUserAddressSet = Types.CalendarUserAddressSet.Default,
                    CalendarHomeSet = CalendarHomeSet.Default,
                    CurrentUserPrincipal = CurrentUserPrincipal.Default,
                    DisplayName = DisplayName.Default
                }
            };

            var content = XmlSerializer.Instance.Serialize(principalRequest);

            var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), requestUri)
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

            var multistatus = XmlSerializer.Instance.Deserialize<Multistatus>(input);

            var calendarUserAddressSets = multistatus.Responses.FirstOrDefault().
                Propstat.Prop.CalendarUserAddressSet.Href.
                Select(url => new CalendarUserAddressSet { Url = url.Value, Preferred = url.Preferred });

            return new CalendarPrincipal
            {
                CalendarUserAddressSet = new List<CalendarUserAddressSet>(calendarUserAddressSets),
                CalendarHomeSet = multistatus.Responses.FirstOrDefault().Propstat.Prop.CalendarHomeSet.Url.Value,
                DisplayName = multistatus.Responses.FirstOrDefault().Propstat.Prop.DisplayName.Value,
                CurrentUserPrincipal = multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value
            };
        }

        private async static Task<PeoplePrincipal> GetPeoplePrincipal(this ConfigurableHttpClient httpClient, string requestUri, CancellationToken cancellationToken)
        {
            var principalRequest = new Propfind()
            {
                Prop = new Prop()
                {
                    AddressBookHomeSet = AddressBookHomeSet.Default,
                    CurrentUserPrincipal = CurrentUserPrincipal.Default,
                    DisplayName = DisplayName.Default
                }
            };
            var content = XmlSerializer.Instance.Serialize(principalRequest);

            var response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), requestUri)
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

            var multistatus = XmlSerializer.Instance.Deserialize<Multistatus>(input);
            return new PeoplePrincipal
            {
                AddressBookHomeSet = multistatus.Responses.FirstOrDefault().Propstat.Prop.AddressBookHomeSet.Url.Value,
                DisplayName = multistatus.Responses.FirstOrDefault().Propstat.Prop.DisplayName.Value,
                CurrentUserPrincipal = multistatus.Responses.FirstOrDefault().Propstat.Prop.CurrentUserPrincipal.Url.Value
            };
        }
    }
}
