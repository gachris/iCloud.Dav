using iCloud.Dav.Core;
using iCloud.Dav.People.Requests;
using System;

namespace iCloud.Dav.People.Resources;

/// <summary>
/// Represents a resource on iCloud for accessing iCloud Drive files for the authenticated user.
/// </summary>
public class CloudGatewayResource
{
    private readonly IClientService _service;

    /// <summary>
    /// Constructs a new <see cref="CloudGatewayResource"/> instance.
    /// </summary>
    /// <param name="service">The client service used for making requests.</param>
    public CloudGatewayResource(IClientService service) => _service = service;

    /// <summary>
    /// Creates a new <see cref="GetRequest"/> instance for retrieving the content of a specified iCloud Drive file.
    /// </summary>
    /// <param name="uri">The URI of the iCloud Drive file to retrieve.</param>
    /// <returns>A new <see cref="GetRequest"/> instance for retrieving the content of the specified iCloud Drive file.</returns>
    public virtual GetRequest Get(Uri uri) => new GetRequest(_service, uri);

    /// <summary>
    /// Represents a request to retrieve the content of an iCloud Drive file.
    /// </summary>
    public class GetRequest : PeopleBaseServiceRequest<byte[]>
    {
        /// <summary>
        /// Constructs a new <see cref="GetRequest"/> instance.
        /// </summary>
        /// <param name="service">The client service used for making requests.</param>
        /// <param name="uri">The URI of the iCloud Drive file to retrieve.</param>
        public GetRequest(IClientService service, Uri uri) : base(service) => RestPath = uri.AbsolutePath;

        /// <inheritdoc/>
        public override string MethodName => "get";

        /// <inheritdoc/>
        public override string HttpMethod => Constants.Get;

        /// <inheritdoc/>
        public override string RestPath { get; }
    }
}