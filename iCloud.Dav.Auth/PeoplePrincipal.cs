using Newtonsoft.Json;

namespace iCloud.Dav.Auth;

/// <summary>
/// 
/// </summary>
public class PeoplePrincipal
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("current_user_principal")]
    public string CurrentUserPrincipal { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("address_book_home_set")]
    public string AddressBookHomeSet { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("display_name")]
    public string DisplayName { get; set; }
}
