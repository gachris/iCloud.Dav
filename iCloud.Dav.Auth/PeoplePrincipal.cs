using Newtonsoft.Json;

namespace iCloud.Dav.Auth;

/// <summary>
/// Represents the principal information for a people user, which includes the current user principal, address book home set, and display name.
/// </summary>
public class PeoplePrincipal
{
    /// <summary>
    /// Gets the current user principal for the people user.
    /// </summary>
    [JsonProperty("current_user_principal")]
    public string CurrentUserPrincipal { get; }

    /// <summary>
    /// Gets the URL of the address book home set for the user.
    /// </summary>
    [JsonProperty("address_book_home_set")]
    public string AddressBookHomeSet { get; }

    /// <summary>
    /// Gets the display name of the user.
    /// </summary>
    [JsonProperty("display_name")]
    public string DisplayName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PeoplePrincipal"/> class with the specified parameters.
    /// </summary>
    /// <param name="currentUserPrincipal">The current user principal for the people user.</param>
    /// <param name="addressBookHomeSet">The URL of the address book home set for the user.</param>
    /// <param name="displayName">The display name of the user.</param>
    public PeoplePrincipal(string currentUserPrincipal, string addressBookHomeSet, string displayName)
    {
        CurrentUserPrincipal = currentUserPrincipal;
        AddressBookHomeSet = addressBookHomeSet;
        DisplayName = displayName;
    }
}