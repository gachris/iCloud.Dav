using iCloud.Dav.Core;
using iCloud.Dav.Core.Response;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Requests;
using System;

namespace iCloud.Dav.People.Resources
{
    /// <summary>
    /// The "Identity Card" collection of methods.
    /// </summary>
    public class IdentityCardResource
    {
        /// <summary>
        /// The service which this resource belongs to.
        /// </summary>
        private readonly IClientService _service;

        /// <summary>
        /// Constructs a new resource.
        /// </summary>
        public IdentityCardResource(IClientService service) => _service = service;

        /// <summary>
        /// Returns the changes on the user's identity card list.
        /// </summary>
        public virtual SyncCollectionRequest SyncCollection(string resourceName) => new SyncCollectionRequest(_service, resourceName);

        /// <summary>
        /// Returns the identity cards on the user's identity card list.
        /// </summary>
        public virtual ListRequest List() => new ListRequest(_service);

        /// <summary>
        /// Returns me-card.
        /// </summary>
        public virtual GetMeCardRequest GetMeCard() => new GetMeCardRequest(_service);

        /// <summary>
        /// Returns events on the specified calendar.
        /// </summary>
        /// <param name="resourceName">Resource Name. To retrieve resource names call the <see cref="List"/> method.</param>
        /// <param name="contactId">Contact identifier. To retrieve contact IDs call the <see cref="PeopleResource.List(string)"/> method.</param>
        public virtual SetMeCardRequest SetMeCard(string resourceName, string contactId) => new SetMeCardRequest(_service, resourceName, contactId);

        /// <summary>
        /// Returns the changes on the user's identity card list.
        /// </summary>
        public class SyncCollectionRequest : PeopleBaseServiceRequest<SyncCollectionList>
        {
            private SyncCollection _body;

            /// <summary>
            /// Constructs a new Sync Collection request.
            /// </summary>
            public SyncCollectionRequest(IClientService service, string resourceName) : base(service)
            {
                SyncLevel = "1";
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Token obtained from the nextSyncToken field returned on the last page of results
            /// from the previous list request. It makes the result of this list request contain
            /// only entries that have changed since then. All cards deleted since the previous
            /// list request will always be in the result. Optional. The default is to return all entries.
            /// </summary>
            public virtual string SyncToken { get; set; }

            public virtual string SyncLevel { get; set; }

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
                    _body = new SyncCollection();
                }

                _body.SyncToken = SyncToken;
                _body.SyncLevel = SyncLevel;

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
        /// Returns the identity cards on the user's identity card list.
        /// </summary>
        public class ListRequest : PeopleBaseServiceRequest<IdentityCardList>
        {
            private object _body;

            /// <summary>
            /// Constructs a new Get request.
            /// </summary>
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
                    _body = new PropFind();
                }
                return _body;
            }
        }

        /// <summary>
        /// Returns me-card from the user's people list.
        /// </summary>
        public class GetMeCardRequest : PeopleBaseServiceRequest<MeCard>
        {
            private PropFind _body;

            /// <summary>
            /// Constructs a new MultiGet request.
            /// </summary>
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
                    _body = new PropFind();
                }
                return _body;
            }
        }

        /// <summary>
        /// Updates me-card on the user's people list.
        /// </summary>
        public class SetMeCardRequest : PeopleBaseServiceRequest<VoidResponse>
        {
            private PropertyUpdate _body;

            /// <summary>
            /// Constructs a new SetMeCard request.
            /// </summary>
            public SetMeCardRequest(IClientService service, string resourceName, string contactId) : base(service)
            {
                ContactId = contactId.ThrowIfNull(nameof(contactId));
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(IdentityCard.ResourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the <see cref="List"/> method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Contact identifier. To retrieve contact IDs call the <see cref="PeopleResource.List(string)"/> method.
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
                    _body = new PropertyUpdate(hrefUri.AbsolutePath);
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
}