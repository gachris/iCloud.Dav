using iCloud.Dav.Core;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Response;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Extensions;
using iCloud.Dav.People.Requests;
using iCloud.Dav.People.WebDav.DataTypes;

namespace iCloud.Dav.People.Resources;

/// <summary>
/// Represents a resource on iCloud for accessing iCloud contacts for the authenticated user.
/// </summary>
public class PeopleResource
{
    private readonly IClientService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="PeopleResource"/> class.
    /// </summary>
    /// <param name="service">The client service used for making requests.</param>
    public PeopleResource(IClientService service) => _service = service;

    /// <summary>
    /// Creates a new <see cref="ListRequest"/> instance for retrieving the list of contact.
    /// </summary>
    /// <param name="resourceName">The name of the resource to retrieve the list from. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="ListRequest"/> instance for retrieving the list of contact.</returns>
    public virtual ListRequest List(string resourceName) => new ListRequest(_service, resourceName);

    /// <summary>
    /// Creates a new <see cref="GetRequest"/> instance for retrieving a specific contact by ID.
    /// </summary>
    /// <param name="contactId">The <see cref="Contact.Id"/> of the contact to retrieve. To retrieve contact IDs, call the <see cref="List"/> method.</param>
    /// <param name="resourceName">The name of the resource where the contact is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="GetRequest"/> instance for retrieving a specific contact by ID.</returns>
    public virtual GetRequest Get(string contactId, string resourceName) => new GetRequest(_service, contactId, resourceName);

    /// <summary>
    /// Creates a new <see cref="MultiGetRequest"/> instance for retrieving multiple contacts by ID.
    /// </summary>
    /// <param name="resourceName">The name of the resource where the contacts are located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <param name="contactIds">The <see cref="Contact.Id"/> array of the contacts to retrieve. To retrieve contact IDs, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="MultiGetRequest"/> instance for retrieving multiple contacts by ID.</returns>
    public virtual MultiGetRequest MultiGet(string resourceName, string[] contactIds) => new MultiGetRequest(_service, resourceName, contactIds);

    /// <summary>
    /// Creates a new <see cref="InsertRequest"/> instance that can insert a new contact.
    /// </summary>            
    /// <param name="body">The contact to insert.</param>
    /// <param name="resourceName">The name of the resource where the contact will be stored. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="InsertRequest"/> instance that can insert a new contact.</returns>
    public virtual InsertRequest Insert(Contact body, string resourceName) => new InsertRequest(_service, body, resourceName);

    /// <summary>
    /// Creates a new <see cref="UpdateRequest"/> instance that can update an existing contact.
    /// </summary>
    /// <param name="body">The body of the request containing the updated contact information.</param>
    /// <param name="contactId">The <see cref="Contact.Id"/> of the contact to update. To retrieve contact IDs, call the <see cref="List"/> method.</param>
    /// <param name="resourceName">The name of the resource where the contact is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="UpdateRequest"/> instance that can update an existing contact.</returns>
    public virtual UpdateRequest Update(Contact body, string contactId, string resourceName) => new UpdateRequest(_service, body, contactId, resourceName);

    /// <summary>
    /// Creates a new <see cref="DeleteRequest"/> instance that can delete an existing contact by ID.
    /// </summary>
    /// <param name="contactId">The <see cref="Contact.Id"/> of the contact to delete. To retrieve contact IDs, call the <see cref="List"/> method.</param>
    /// <param name="resourceName">The name of the resource where the contact is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
    /// <returns>A new <see cref="DeleteRequest"/> instance that can delete an existing contact by ID.</returns>
    public virtual DeleteRequest Delete(string contactId, string resourceName) => new DeleteRequest(_service, contactId, resourceName);

    /// <summary>
    /// Represents a request to retrieve a list of contacts from iCloud.
    /// </summary>
    public class ListRequest : PeopleBaseServiceRequest<ContactList>
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
        public override string MethodName => "list";

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
                    PropFilters =
                    [
                        new PropFilter()
                        {
                            Name = "X-ADDRESSBOOKSERVER-KIND",
                            IsNotDefined = true,
                            TextMatches =
                            [
                                new TextMatch
                                {
                                    Collation = "i;unicode-casemap",
                                    NegateCondition = "yes",
                                    MatchType = "equals",
                                    SearchText = "group"
                                }
                            ]
                        }
                    ]
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
    /// Represents a request to get a single contact by ID from iCloud.
    /// </summary>
    public class GetRequest : PeopleBaseServiceRequest<Contact>
    {
        /// <summary>
        /// Constructs a new <see cref="GetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="contactId">The <see cref="Contact.Id"/> of the contact to retrieve. To retrieve contact IDs, call the <see cref="List"/> method.</param>
        /// <param name="resourceName">The name of the resource where the contact is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        public GetRequest(IClientService service, string contactId, string resourceName) : base(service)
        {
            ContactId = contactId.ThrowIfNullOrEmpty(nameof(contactId));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact ID.
        /// </summary>
        [RequestParameter("contactId", RequestParameterType.Path)]
        public virtual string ContactId { get; }

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Get;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{contactId}.vcf";

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("contactId", new Parameter("contactId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to retrieve multiple iCloud contacts by ID.
    /// </summary>
    public class MultiGetRequest : PeopleBaseServiceRequest<ContactList>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="MultiGetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="resourceName">The name of the resource where the contacts are located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        /// <param name="contactIds">The <see cref="Contact.Id"/> array of the contacts to retrieve. To retrieve contact IDs, call the <see cref="List"/> method.</param>
        public MultiGetRequest(IClientService service, string resourceName, string[] contactIds) : base(service)
        {
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            ContactIds = contactIds.ThrowIfNull(nameof(contactIds));
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact IDs.
        /// </summary>
        public virtual string[] ContactIds { get; }

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
                Hrefs = ContactIds.Select(contactId => new Href() { Value = Service.HttpClientInitializer.GetAddressBookFullHref(ResourceName, contactId) }).ToArray()
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
    /// Represents a request to insert a contact into iCloud.
    /// </summary>
    public class InsertRequest : PeopleBaseServiceRequest<HeaderMetadataResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="InsertRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="body">The contact to insert.</param>
        /// <param name="resourceName">The name of the resource where the contact will be stored. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        public InsertRequest(IClientService service, Contact body, string resourceName) : base(service)
        {
            Body = body.ThrowIfNull(nameof(body));
            ContactId = !string.IsNullOrEmpty(body.Id) ? body.Id : Guid.NewGuid().ToString();
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            ETagAction = ETagAction.IfNoneMatch;
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact ID.
        /// </summary>
        [RequestParameter("contactId", RequestParameterType.Path)]
        public virtual string ContactId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private Contact Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "insert";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Put;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{contactId}.vcf";

        /// <inheritdoc/>
        public override string ContentType => Constants.TEXT_VCARD;

        /// <inheritdoc/>
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("contactId", new Parameter("contactId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to update an existing contact in iCloud.
    /// </summary>
    public class UpdateRequest : PeopleBaseServiceRequest<HeaderMetadataResponse>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="UpdateRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="body">The body of the request containing the updated contact information.</param>
        /// <param name="contactId">The <see cref="Contact.Id"/> of the contact to update. To retrieve contact IDs, call the <see cref="List"/> method.</param>
        /// <param name="resourceName">The name of the resource where the contact is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        public UpdateRequest(IClientService service, Contact body, string contactId, string resourceName) : base(service)
        {
            Body = body.ThrowIfNull(nameof(body));
            ContactId = contactId.ThrowIfNullOrEmpty(nameof(contactId));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            ETagAction = ETagAction.IfMatch;
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact ID.
        /// </summary>
        [RequestParameter("contactId", RequestParameterType.Path)]
        public virtual string ContactId { get; }

        /// <summary>
        /// Gets the body of this request.
        /// </summary>
        private Contact Body { get; }

        /// <inheritdoc/>
        public override string MethodName => "update";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Put;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{contactId}.vcf";

        /// <inheritdoc/>
        public override string ContentType => Constants.TEXT_VCARD;

        /// <inheritdoc/>
        protected override object GetBody() => _body ??= Body.SerializeToString();

        /// <summary>Initializes Update parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("contactId", new Parameter("contactId", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to delete a contact from iCloud.
    /// </summary>
    public class DeleteRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        /// <summary>
        /// Constructs a new <see cref="DeleteRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="contactId">The <see cref="Contact.Id"/> of the contact to delete. To retrieve contact IDs, call the <see cref="List"/> method.</param>
        /// <param name="resourceName">The name of the resource where the contact is located. To retrieve resource names, call the <see cref="IdentityCardResource.List"/> method.</param>
        public DeleteRequest(IClientService service, string contactId, string resourceName) : base(service)
        {
            ContactId = contactId.ThrowIfNullOrEmpty(nameof(contactId));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact IDs.
        /// </summary>
        [RequestParameter("contactId", RequestParameterType.Path)]
        public virtual string ContactId { get; }

        /// <inheritdoc/>
        public override string MethodName => "delete";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Delete;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}/{contactId}.vcf";

        /// <summary>Initializes Delete parameter list.</summary>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
            RequestParameters.Add("contactId", new Parameter("contactId", "path", true));
        }
    }
}