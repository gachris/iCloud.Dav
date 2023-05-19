using System.Collections.Generic;

namespace iCloud.Dav.Auth.CardDav.Types
{
    /// <summary>
    /// Represents a response to a PROPFIND request.
    /// </summary>
    internal sealed class Response
    {
        /// <summary>
        /// Gets the Href of the resource to which this response pertains.
        /// </summary>
        public string Href { get; }

        /// <summary>
        /// Gets the URL of the principal associated with the resource.
        /// </summary>
        public string CurrentUserPrincipal { get; }

        /// <summary>
        /// Gets the URL of the calendar home collection for the principal.
        /// </summary>
        public string CalendarHomeSet { get; }

        /// <summary>
        /// Gets the URL of the address book home collection for the principal.
        /// </summary>
        public string AddressBookHomeSet { get; }

        /// <summary>
        /// Gets the display name of the resource.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the set of calendar user addresses associated with the resource.
        /// </summary>
        public IEnumerable<CalendarUserAddress> CalendarUserAddressSet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="href">The Href of the resource to which this response pertains.</param>
        /// <param name="currentUserPrincipal">The URL of the principal associated with the resource.</param>
        /// <param name="calendarHomeSet">The URL of the calendar home collection for the principal.</param>
        /// <param name="addressBookHomeSet">The URL of the address book home collection for the principal.</param>
        /// <param name="displayName">The display name of the resource.</param>
        /// <param name="calendarUserAddressSet">The set of calendar user addresses associated with the resource.</param>
        public Response(string href,
                        string currentUserPrincipal,
                        string calendarHomeSet,
                        string addressBookHomeSet,
                        string displayName,
                        IEnumerable<CalendarUserAddress> calendarUserAddressSet)
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