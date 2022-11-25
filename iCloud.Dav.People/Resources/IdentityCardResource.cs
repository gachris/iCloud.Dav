using iCloud.Dav.Core;
using iCloud.Dav.People.CardDav.Types;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Requests;

namespace iCloud.Dav.People.Resources;

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
    /// Returns the identity cards on the user's identity card list.
    /// </summary>
    public virtual ListRequest List() => new(_service);

    /// <summary>
    /// Returns the identity cards on the user's identity card list.
    /// </summary>
    public class ListRequest : PeopleBaseServiceRequest<IdentityCardResponse>
    {
        private object? _body;

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
        protected override object GetBody() => _body ??= new PropFind();
    }
}
