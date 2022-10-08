namespace iCloud.Dav.People.Types;

public class IdentityCard
{
    #region Properties

    /// <summary>
    /// The resource name of the IdentityCard
    /// </summary>
    public virtual string ResourceName { get; set; }

    /// <summary>
    /// The unique id of the IdentityCard
    /// </summary>
    public virtual string UniqueId { get; set; }

    /// <summary>
    /// The url of the IdentityCard
    /// </summary>
    public virtual string Url { get; set; }

    #endregion
}
