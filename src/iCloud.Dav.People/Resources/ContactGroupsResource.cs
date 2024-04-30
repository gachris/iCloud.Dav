using System.Linq;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Response;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.Requests;
using iCloud.Dav.People.WebDav.DataTypes;

namespace iCloud.Dav.People.Resources;

/// <summary>
/// Represents a resource on iCloud for accessing contact groups for the authenticated user.
/// </summary>
public class ContactGroupsResource
{
    private readonly IClientService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactGroupsResource"/> class.
    /// </summary>
    /// <param name="service">The client service used for making requests.</param>
    public ContactGroupsResource(IClientService service) => _service = service;

    /// <summary>
    /// Creates a new <see cref="ListRequest"/> instance for retrieving the list of contact group.
    /// </summary>
    /// <param name="resourceName">The name of the resource to retrieve the list from. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="ListRequest"/> instance for retrieving the list of contact group.</returns>
    public virtual ListRequest List(string resourceName) => new ListRequest(_service, resourceName);

    /// <summary>
    /// Creates a new <see cref="GetRequest"/> instance for retrieving a specific contact group by ID.
    /// </summary>
    /// <param name="contactGroupId">The ID of the contact group to retrieve. To retrieve contact group IDs, call the <see cref="List"/> method.</param>
    /// <param name="resourceName">The name of the resource where the contact group is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="GetRequest"/> instance for retrieving a specific contact group by ID.</returns>
    public virtual GetRequest Get(string contactGroupId, string resourceName) => new GetRequest(_service, contactGroupId, resourceName);

    /// <summary>
    /// Creates a new <see cref="MultiGetRequest"/> instance for retrieving multiple contact groups by ID.
    /// </summary>
    /// <param name="resourceName">The name of the resource where the contact groups are located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <param name="contactGroupIds">The IDs of the contact groups to retrieve. To retrieve contact group IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="MultiGetRequest"/> instance for retrieving multiple contact groups by ID.</returns>
    public virtual MultiGetRequest MultiGet(string resourceName, string[] contactGroupIds) => new MultiGetRequest(_service, resourceName, contactGroupIds);

    /// <summary>
    /// Creates a new <see cref="InsertRequest"/> instance that can insert a new contact group.
    /// </summary>            
    /// <param name="body">The contact group to insert.</param>
    /// <param name="resourceName">The name of the resource where the contact group will be stored. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="InsertRequest"/> instance that can insert a new contact group.</returns>
    public virtual InsertRequest Insert(ContactGroup body, string resourceName) => new InsertRequest(_service, body, resourceName);

    /// <summary>
    /// Creates a new <see cref="UpdateRequest"/> instance that can update an existing contact group.
    /// </summary>
    /// <param name="body">The body of the request containing the updated contact group information.</param>
    /// <param name="resourceName">The name of the resource where the contact group is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="UpdateRequest"/> instance that can update an existing contact group.</returns>
    public virtual UpdateRequest Update(ContactGroup body, string resourceName) => new UpdateRequest(_service, body, resourceName);

    /// <summary>
    /// Creates a new <see cref="DeleteRequest"/> instance that can delete an existing contact group by ID.
    /// </summary>
    /// <param name="contactGroupId">The ID of the contact group to delete. To retrieve contact group IDs, call the <see cref="List"/> method.</param>
    /// <param name="resourceName">The name of the resource where the contact group is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="DeleteRequest"/> instance that can delete an existing contact group by ID.</returns>
    public virtual DeleteRequest Delete(string contactGroupId, string resourceName) => new DeleteRequest(_service, contactGroupId, resourceName);

    /// <summary>
    /// Represents a request to retrieve a list of contact groups from iCloud.
    /// </summary>
    public class ListRequest : PeopleBaseServiceRequest<ContactGroupList>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="ListRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="resourceName">The name of the resource to retrieve the list from. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        public ListRequest(IClientService service, string resourceName) : base(service) => ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <inheritdoc/>
        public override string MethodName => Constants.Report;

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Report;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}";

        /// <inheritdoc/>
        public override string Depth => "1";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            return _body ??= new AddressbookQuery
            {
                Prop = new Prop(),
                Filter = new Filters
                {
                    Test = "anyof",
                    PropFilters = new[]
                    {
                        new PropFilter()
                        {
                            Name = "X-ADDRESSBOOKSERVER-KIND",
                            TextMatches = new[]
                            {
                                new TextMatch
                                {
                                    Collation = "i;unicode-casemap",
                                    NegateCondition = "no",
                                    MatchType = "equals",
                                    SearchText = "group"
                                }
                            }
                        }
                    }
                },
                Limit = new Limit()
                {
                    NResults = 50001
                }
            };
        }

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to get a single contact group by ID from iCloud.
    /// </summary>
    public class GetRequest : PeopleBaseServiceRequest<ContactGroup>
    {
        /// <summary>
        /// Constructs a new <see cref="GetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="contactGroupId">The ID of the contact group to retrieve. To retrieve contact group IDs, call the <see cref="List"/> method.</param>
        /// <param name="resourceName">The name of the resource where the contact group is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        public GetRequest(IClientService service, string contactGroupId, string resourceName) : base(service)
        {
            ContactGroupId = contactGroupId.ThrowIfNullOrEmpty(nameof(ContactGroup.Id));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact group ID.
        /// </summary>
        [RequestParameter("contactGroupId", RequestParameterType.Path)]
        public virtual string ContactGroupId { get; }

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Get;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{contactGroupId}.vcf";

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("contactGroupId", new Parameter("contactGroupId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to retrieve multiple iCloud contact groups by ID.
    /// </summary>
    public class MultiGetRequest : PeopleBaseServiceRequest<ContactGroupList>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="MultiGetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="resourceName">The name of the resource where the contact groups are located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        /// <param name="contactGroupIds">The IDs of the contact groups to retrieve. To retrieve contact group IDs, call the <see cref="List"/> method.</param>
        public MultiGetRequest(IClientService service, string resourceName, string[] contactGroupIds) : base(service)
        {
            ContactGroupIds = contactGroupIds.ThrowIfNull(nameof(contactGroupIds));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact group IDs.
        /// </summary>
        public virtual string[] ContactGroupIds { get; }

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Report;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}";

        /// <inheritdoc/>
        public override string Depth => "1";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            return _body ??= new AddressbookMultiget()
            {
                Prop = new Prop(),
                Hrefs = ContactGroupIds.Select(contactGroupId => new Href() { Value = Service.HttpClientInitializer.GetAddressBookFullHref(ResourceName, contactGroupId) }).ToArray()
            };
        }

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to insert a contact group into iCloud.
    /// </summary>
    public class InsertRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="InsertRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="body">The contact group to insert.</param>
        /// <param name="resourceName">The name of the resource where the contact group will be stored. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        public InsertRequest(IClientService service, ContactGroup body, string resourceName) : base(service)
        {
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
            Body = body.ThrowIfNull(nameof(ContactGroup));
            ContactGroupId = body.Id.ThrowIfNull(nameof(ContactGroup.Id));
            ETagAction = ETagAction.IfMatch;
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact group ID.
        /// </summary>
        [RequestParameter("contactGroupId", RequestParameterType.Path)]
        public virtual string ContactGroupId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private ContactGroup Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "insert";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Put;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{contactGroupId}.vcf";

        /// <inheritdoc/>
        public override string ContentType => Constants.TEXT_VCARD;

        /// <inheritdoc/>
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("contactGroupId", new Parameter("contactGroupId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to update an existing contact group in iCloud.
    /// </summary>
    public class UpdateRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="UpdateRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="body">The body of the request containing the updated contact group information.</param>
        /// <param name="resourceName">The name of the resource where the contact group is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        public UpdateRequest(IClientService service, ContactGroup body, string resourceName) : base(service)
        {
            Body = body.ThrowIfNull(nameof(ContactGroup));
            ContactGroupId = body.Id.ThrowIfNullOrEmpty(nameof(ContactGroup.Id));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
            ETagAction = ETagAction.IfMatch;
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact group ID.
        /// </summary>
        [RequestParameter("contactGroupId", RequestParameterType.Path)]
        public virtual string ContactGroupId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private ContactGroup Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "update";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Put;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{contactGroupId}.vcf";

        /// <inheritdoc/>
        public override string ContentType => Constants.TEXT_VCARD;

        /// <inheritdoc/>
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("contactGroupId", new Parameter("contactGroupId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to delete a contact group from iCloud.
    /// </summary>
    public class DeleteRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        /// <summary>
        /// Constructs a new <see cref="DeleteRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="contactGroupId">The identifier of the contact group to delete. To retrieve contact group IDs, call the <see cref="List"/> method.</param>
        /// <param name="resourceName">The name of the resource where the contact group is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        public DeleteRequest(IClientService service, string contactGroupId, string resourceName) : base(service)
        {
            ContactGroupId = contactGroupId.ThrowIfNullOrEmpty(nameof(ContactGroup.Id));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact group ID.
        /// </summary>
        [RequestParameter("contactGroupId", RequestParameterType.Path)]
        public virtual string ContactGroupId { get; }

        /// <inheritdoc/>
        public override string MethodName => "delete";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Delete;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{contactGroupId}.vcf";

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("contactGroupId", new Parameter("contactGroupId", "path", true));
        }
    }
}