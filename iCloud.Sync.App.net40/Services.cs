using iCloud.Dav.Auth;
using iCloud.Dav.Auth.Utils.Store;
using iCloud.Dav.Calendar.Services;
using iCloud.Dav.Core.Services;
using iCloud.Dav.People.Services;
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
                IDataStore dataStore = new FileDataStore("icloudStore");

                UserCredential credential = AuthorizationBroker.AuthorizeAsync("gachris",
                    Credentials.NetworkCredential,
                    CancellationToken.None,
                    dataStore)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

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
                IDataStore dataStore = new FileDataStore("icloudStore");

                UserCredential credential = AuthorizationBroker.AuthorizeAsync("gachris",
                    Credentials.NetworkCredential,
                    CancellationToken.None,
                    dataStore)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                _calendarService = new CalendarService(new BaseClientService.Initializer()
                {
                    ApplicationName = "iCloud.SyncApp",
                    HttpClientInitializer = credential
                });
            }
            return _calendarService;
        }
    }
}
