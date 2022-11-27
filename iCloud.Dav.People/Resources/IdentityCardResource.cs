using iCloud.Dav.Core;
using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Requests;

namespace iCloud.Dav.People.Resources
{
    /// <summary>
    /// The identity card collection of methods.
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
        /// Returns the changes on the user's identity card list.
        /// </summary>
        public class SyncCollectionRequest : PeopleBaseServiceRequest<IdentityCardList>
        {
            private SyncCollection _body;

            /// <summary>
            /// Constructs a new Sync Collection request.
            /// </summary>
            public SyncCollectionRequest(IClientService service, string resourceName) : base(service)
            {
                SyncLevel = "1";
                ResourceName = resourceName.ThrowIfNullOrEmpty(nameof(resourceName));
            }

            /// <summary>
            /// Resource Name. To retrieve resource names call the identityCard.list method.
            /// </summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            /// <summary>
            /// Optional. A sync token, received from a previous response `next_sync_token` Provide
            /// this to retrieve only the resources changed since the last request. When syncing,
            /// all other parameters provided to `people.list` must match the first
            /// call that provided the sync token. More details about sync behavior at `people.list`.
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

            /// <summary>
            /// Optional. A sync token, received from a previous response `next_sync_token` Provide
            /// this to retrieve only the resources changed since the last request. When syncing,
            /// all other parameters provided to `people.list` must match the first
            /// call that provided the sync token. More details about sync behavior at `people.list`.
            /// </summary>
            public virtual string SyncToken { get; set; }

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
    }
}