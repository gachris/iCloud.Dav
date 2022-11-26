using System.Collections.Generic;

namespace iCloud.Dav.Auth.CardDav.Types
{
    internal sealed class Response
    {
        public string Href { get; }

        public string CurrentUserPrincipal { get; }

        public string CalendarHomeSet { get; }

        public string AddressBookHomeSet { get; }

        public string DisplayName { get; }

        public IEnumerable<CalendarUserAddress> CalendarUserAddressSet { get; }

        public Response(string href, string currentUserPrincipal, string calendarHomeSet, string addressBookHomeSet, string displayName, IEnumerable<CalendarUserAddress> calendarUserAddressSet)
        {
            Href = href;
            CurrentUserPrincipal = currentUserPrincipal;
            CalendarHomeSet = calendarHomeSet;
            AddressBookHomeSet = addressBookHomeSet;
            DisplayName = displayName;
            CalendarUserAddressSet = calendarUserAddressSet;
        }
    }
}