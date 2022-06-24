using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Utils.Store;
using iCloud.Dav.ICalendar.Services;
using iCloud.Dav.Core.Services;
using iCloud.Dav.People.Services;
using System.Net;
using System.Threading;

namespace iCloud.Sync.App
{
    public class Services
    {
        private static PeopleService _peopleService;
        public static PeopleService GetPeopleService()
        {
            if (_peopleService == null)
            {
                UserCredential credential = GetCredential();

                _peopleService = new PeopleService(new BaseClientService.Initializer()
                {
                    ApplicationName = "iCloud.SyncApp",
                    HttpClientInitializer = credential
                });
            }
            return _peopleService;
        }

        private static CalendarService _calendarService;
        public static CalendarService GetCalendarService()
        {
            if (_calendarService == null)
            {
                UserCredential credential = GetCredential();

                _calendarService = new CalendarService(new BaseClientService.Initializer()
                {
                    ApplicationName = "iCloud.SyncApp",
                    HttpClientInitializer = credential
                });
            }
            return _calendarService;
        }

        private static UserCredential GetCredential()
        {
            IDataStore dataStore = new FileDataStore("icloudStore");

            UserCredential credential = AuthorizationBroker.AuthorizeAsync("gachris",
                NetworkCredential,
                CancellationToken.None,
                dataStore)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            // use this if you want to remove stored credentials
            //credential.RevokeTokenAsync(CancellationToken.None).ConfigureAwait(false)
            //    .GetAwaiter()
            //    .GetResult();

            return credential;
        }

        private static NetworkCredential _networkCredential;
        private static NetworkCredential NetworkCredential
        {
            get
            {
                if (_networkCredential == null)
                {
                    _networkCredential = new NetworkCredential("xxxxxx@xxxx.com", "xxxx-xxxx-xxxx-xxxx");
                }
                return _networkCredential;
            }
        }
    }
}
