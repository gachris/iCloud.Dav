using Newtonsoft.Json;
using System.Collections.Generic;

namespace iCloud.Dav.Auth
{
    /// <summary>
    /// Represents the principal information for a calendar user.
    /// </summary>
    public class CalendarPrincipal
    {
        /// <summary>
        /// Gets or sets the current user principal for the calendar.
        /// </summary>
        [JsonProperty("current_user_principal")]
        public string CurrentUserPrincipal { get; }

        /// <summary>
        /// Gets or sets the URL of the calendar home set for the user.
        /// </summary>
        [JsonProperty("calendar_home_set")]
        public string CalendarHomeSet { get; }

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; }

        /// <summary>
        /// Gets or sets the list of calendar user address sets for the user.
        /// </summary>
        [JsonProperty("calendar_user_address_set")]
        public IEnumerable<CalendarUserAddressSet> CalendarUserAddressSet { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarPrincipal"/> class with the specified parameters.
        /// </summary>
        /// <param name="currentUserPrincipal">The current user principal for the calendar.</param>
        /// <param name="calendarHomeSet">The URL of the calendar home set for the user.</param>
        /// <param name="displayName">The display name of the user.</param>
        /// <param name="calendarUserAddressSet">The list of calendar user address sets for the user.</param>
        public CalendarPrincipal(string currentUserPrincipal, string calendarHomeSet, string displayName, IEnumerable<CalendarUserAddressSet> calendarUserAddressSet)
        {
            CurrentUserPrincipal = currentUserPrincipal;
            CalendarHomeSet = calendarHomeSet;
            DisplayName = displayName;
            CalendarUserAddressSet = calendarUserAddressSet;
        }
    }
}