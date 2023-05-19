using iCloud.Dav.Core;
using iCloud.Dav.People.Resources;

namespace iCloud.Dav.People.Services
{
    /// <summary>
    /// Represents the service to interact with the Apple iCloud Gateway.
    /// </summary>
    public class CloudGatewayService : BaseClientService
    {
        /// <summary>
        /// The cloud gateway service version.
        /// </summary>
        public const string Version = "v1";

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudGatewayService"/> class.
        /// </summary>
        /// <param name="initializer">The initializer to use for the service.</param>
        public CloudGatewayService(Initializer initializer) : base(initializer) => CloudGateway = new CloudGatewayResource(this);

        /// <inheritdoc/>
        public override string Name => "cloud_gateway";

        /// <inheritdoc/>
        public override string BasePath => "https://gateway.icloud.com/";

        /// <summary>
        /// Gets the <see cref="CloudGatewayResource"/>.
        /// </summary>
        public virtual CloudGatewayResource CloudGateway { get; }
    }
}