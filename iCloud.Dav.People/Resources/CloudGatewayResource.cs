using iCloud.Dav.Core;
using iCloud.Dav.People.Requests;
using System;

namespace iCloud.Dav.People.Resources
{
    /// <summary>
    /// The "people" collection of methods.
    /// </summary>
    public class CloudGatewayResource
    {
        /// <summary>
        /// The service which this resource belongs to.
        /// </summary>
        private readonly IClientService _service;

        /// <summary>
        /// Constructs a new resource.
        /// </summary>
        public CloudGatewayResource(IClientService service) => _service = service;

        /// <summary>
        /// Returns contact photo from gateway.
        /// </summary>
        /// <param name="uri">Uri to retrieve contact photo.</param>
        public virtual GetContactPhotoRequest GetContactPhoto(Uri uri) => new GetContactPhotoRequest(_service, uri);

        /// <summary>
        /// Returns contact photo from gateway.
        /// </summary>
        public class GetContactPhotoRequest : PeopleBaseServiceRequest<byte[]>
        {
            /// <summary>
            /// Constructs a new Get request.
            /// </summary>
            public GetContactPhotoRequest(IClientService service, Uri uri) : base(service) => RestPath = uri.AbsolutePath;

            /// <inheritdoc/>
            public override string MethodName => "get";

            /// <inheritdoc/>
            public override string HttpMethod => Constants.Get;

            /// <inheritdoc/>
            public override string RestPath { get; }
        }
    }
}