using iCloud.Dav.Core.Enums;
using iCloud.Dav.Core.Services;
using iCloud.Dav.People.Resources;

namespace iCloud.Dav.People.Services
{
    public class PeopleService : BaseClientService
    {
        public const string Version = "v1";

        public PeopleService() : this(new Initializer())
        {
        }

        public PeopleService(Initializer initializer) : base(initializer)
        {
            People = new PeopleResource(this);
            ContactGroups = new ContactGroupsResource(this);
            IdentityCard = new IdentityCardResource(this);
            BaseUri = initializer.HttpClientInitializer.GetUriHomeSet(PrincipalHomeSet.AddressBookHomeSet);
        }

        public override string Name => "people";

        public override string BaseUri { get; }

        public override string BasePath { get; }

        /// <summary>Gets the People resource.</summary>
        public virtual PeopleResource People { get; }

        /// <summary>Gets the ContactGroups resource.</summary>
        public virtual ContactGroupsResource ContactGroups { get; }

        /// <summary>Gets the Card resource.</summary>
        public virtual IdentityCardResource IdentityCard { get; }
    }
}