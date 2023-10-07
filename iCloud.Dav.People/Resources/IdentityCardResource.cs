using iCloud.Dav.Core;
using iCloud.Dav.Core.Extensions;
using iCloud.Dav.Core.Response;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Requests;
using iCloud.Dav.People.WebDav.DataTypes;
using System;

namespace iCloud.Dav.People.Resources;

/// <summary>
/// Represents a resource on iCloud for accessing card resources for the authenticated user.
/// </summary>
public class IdentityCardResource
{
    private readonly IClientService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityCardResource"/> class.
    /// </summary>
    /// <param name="service">The client service used for making requests.</param>
    public IdentityCardResource(IClientService service) => _service = service;

    /// <summary>
    /// Creates a new <see cref="GetSyncTokenRequest"/> instance for retrieving the next sync token for the contacts or contact groups on iCloud.
    /// </summary>
    /// <param name="resourceName">The name of the resource where the contacts or contact groups are stored. To retrieve resource names, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="GetSyncTokenRequest"/> instance for retrieving the next sync token for the contacts or contact groups on iCloud.</returns>
    public virtual GetSyncTokenRequest GetSyncToken(string resourceName) => new GetSyncTokenRequest(_service, resourceName);

    /// <summary>
    /// Creates a new <see cref="SyncCollectionRequest"/> instance for retrieving changes from the last sync token associated with the specified resource name.
    /// </summary>
    /// <param name="resourceName">The name of the resource where the contacts or contact groups are stored. To retrieve resource names, call the <see cref="List"/> method.</param>
    /// <returns>A new <see cref="SyncCollectionRequest"/> instance for retrieving changes from the last sync token associated with the specified resource name.</returns>
    public virtual SyncCollectionRequest SyncCollection(string resourceName) => new SyncCollectionRequest(_service, resourceName);

    /// <summary>
    /// Creates a new <see cref="ListRequest"/> instance for retrieving the list of resource names.
    /// </summary>
    /// <returns>A new <see cref="ListRequest"/> instance for retrieving the list of resource names.</returns>
    public virtual ListRequest List() => new ListRequest(_service);

    /// <summary>
    /// Creates a new <see cref="GetMeCardRequest"/> instance for retrieving me-card value.
    /// </summary>
    /// <returns>A new <see cref="GetMeCardRequest"/> instance for retrieving me-card value.</returns>
    public virtual GetMeCardRequest GetMeCard() => new GetMeCardRequest(_service);

    /// <summary>
    /// Creates a new <see cref="SetMeCardRequest"/> instance that can update the me-card property.
    /// </summary>
    /// <param name="resourceName">The name of the resource where the contact is located. To retrieve resource names, call the <see cref="List"/> method.</param>
    /// <param name="contactId">The ID of the contact to set as me-card. To retrieve contact IDs, call the <see cref="PeopleResource.List"/> method.</param>
    /// <returns>A new <see cref="SetMeCardRequest"/> instance that can update the me-card property.</returns>
    public virtual SetMeCardRequest SetMeCard(string resourceName, string contactId) => new SetMeCardRequest(_service, resourceName, contactId);

    /// <summary>
    /// Represents a request to get next sync token for the contacts or contact groups on iCloud.
    /// </summary>
    public class GetSyncTokenRequest : PeopleBaseServiceRequest<DataTypes.SyncToken>
    {
        private PropFind _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSyncTokenRequest"/> class.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="resourceName">The name of the resource where the contacts or contact groups are stored. To retrieve resource names, call the <see cref="List"/> method.</param>
        public GetSyncTokenRequest(IClientService service, string resourceName) : base(service)
        {
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Propfind;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}";

        /// <inheritdoc/>
        public override string Depth => "0";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            if (_body == null)
            {
                _body = new PropFind()
                {
                    Prop = new Prop()
                    {
                        SyncToken = new WebDav.DataTypes.SyncToken(),
                        GetETag = new GetETag()
                    }
                };
            }

            return _body;
        }

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to synchronize a collection of contacts or contact groups on iCloud.
    /// </summary>
    public class SyncCollectionRequest : PeopleBaseServiceRequest<SyncCollectionList>
    {
        private SyncCollection _body;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncCollectionRequest"/> class.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="resourceName">The name of the resource where the contacts or contact groups are stored. To retrieve resource names, call the <see cref="List"/> method.</param>
        public SyncCollectionRequest(IClientService service, string resourceName) : base(service)
        {
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets or sets the synchronization token.
        /// </summary>
        public virtual string SyncToken { get; set; }

        /// <inheritdoc/>
        public override string MethodName => "list";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Report;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}";

        /// <inheritdoc/>
        public override string Depth => "0";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            if (_body == null)
            {
                _body = new SyncCollection() { Prop = new Prop() };
            }

            _body.SyncToken = new WebDav.DataTypes.SyncToken() { Value = SyncToken };
            _body.SyncLevel = new SyncLevel() { Value = "1" };

            return _body;
        }

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
        }
    }

    /// <summary>
    /// Represents a request to retrieve a list of resource names from iCloud.
    /// </summary>
    public class ListRequest : PeopleBaseServiceRequest<IdentityCardList>
    {
        private object _body;

        /// <summary>
        /// Constructs a new <see cref="ListRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        public ListRequest(IClientService service) : base(service)
        {
        }

        /// <inheritdoc/>
        public override string MethodName => "list";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Propfind;

        /// <inheritdoc/>
        public override string RestPath => string.Empty;

        /// <inheritdoc/>
        public override string Depth => "1";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            if (_body == null)
            {
                _body = new PropFind()
                {
                    Prop = new Prop()
                };
            }
            return _body;
        }
    }

    /// <summary>
    /// Represents a request to retrieve me-card value.
    /// </summary>
    public class GetMeCardRequest : PeopleBaseServiceRequest<DataTypes.MeCard>
    {
        private PropFind _body;

        /// <summary>
        /// Constructs a new <see cref="GetMeCardRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        public GetMeCardRequest(IClientService service) : base(service)
        {
        }

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Propfind;

        /// <inheritdoc/>
        public override string RestPath => string.Empty;

        /// <inheritdoc/>
        protected override object GetBody()
        {
            if (_body == null)
            {
                _body = new PropFind()
                {
                    Prop = new Prop()
                    {
                        MeCard = new WebDav.DataTypes.MeCard()
                    }
                };
            }
            return _body;
        }
    }

    /// <summary>
    /// Represents a request to update me-card property.
    /// </summary>
    public class SetMeCardRequest : PeopleBaseServiceRequest<VoidResponse>
    {
        private PropertyUpdate _body;

        /// <summary>
        /// Constructs a new <see cref="SetMeCardRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="resourceName">The name of the resource where the contact is located. To retrieve resource names, call the <see cref="List"/> method.</param>
        /// <param name="contactId">The ID of the contact to set as me-card. To retrieve contact IDs, call the <see cref="PeopleResource.List"/> method.</param>
        public SetMeCardRequest(IClientService service, string resourceName, string contactId) : base(service)
        {
            ContactId = contactId.ThrowIfNull(nameof(contactId));
            ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
        }

        /// <summary>
        /// Gets the resource name.
        /// </summary>
        [RequestParameter("resourceName", RequestParameterType.Path)]
        public virtual string ResourceName { get; }

        /// <summary>
        /// Gets the contact ID.
        /// </summary>
        public virtual string ContactId { get; }

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Proppatch;

        /// <inheritdoc/>
        public override string RestPath => "{resourceName}";

        /// <inheritdoc/>
        protected override object GetBody()
        {
            if (_body == null)
            {
                var hrefUri = new Uri(Service.HttpClientInitializer.GetUri(PrincipalHomeSet.AddressBook), string.Concat(ResourceName, "/", ContactId, ".vcf"));
                _body = new PropertyUpdate()
                {
                    Prop = new Prop()
                    {
                        MeCard = new WebDav.DataTypes.MeCard()
                        {
                            Href = new Href() { Value = hrefUri.AbsolutePath }
                        }
                    }
                };
            }
            return _body;
        }

        /// <inheritdoc/>
        protected override void InitParameters()
        {
            base.InitParameters();

            RequestParameters.Add("resourceName", new Parameter("resourceName", "path", true));
        }
    }
}