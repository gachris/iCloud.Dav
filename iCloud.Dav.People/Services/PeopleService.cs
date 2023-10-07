using iCloud.Dav.Core;
using iCloud.Dav.People.Resources;

namespace iCloud.Dav.People.Services;

/// <summary>
/// Represents the service to interact with the Apple iCloud People.
/// </summary>
public class PeopleService : BaseClientService
{
    /// <summary>
    /// The people service version.
    /// </summary>
    public const string Version = "v1";

    /// <summary>
    /// Initializes a new instance of the <see cref="PeopleService"/> class.
    /// </summary>
    /// <param name="initializer">The initializer to use for the service.</param>
    public PeopleService(Initializer initializer) : base(initializer)
    {
        People = new PeopleResource(this);
        ContactGroups = new ContactGroupsResource(this);
        IdentityCard = new IdentityCardResource(this);
        BasePath = initializer.HttpClientInitializer.GetUri(PrincipalHomeSet.AddressBook).ToString();
    }

    /// <inheritdoc/>
    public override string Name => "people";

    /// <inheritdoc/>
    public override string BasePath { get; }

    /// <summary>
    /// Gets the <see cref="PeopleResource"/>.
    /// </summary>
    public virtual PeopleResource People { get; }

    /// <summary>
    /// Gets the <see cref="ContactGroupsResource"/>.
    /// </summary>
    public virtual ContactGroupsResource ContactGroups { get; }

    /// <summary>
    /// Gets the <see cref="IdentityCardResource"/>.
    /// </summary>
    public virtual IdentityCardResource IdentityCard { get; }
}