using iCloud.Dav.Core;
using iCloud.Dav.People.Resources;

namespace iCloud.Dav.People.Services;

public class CloudGatewayService : BaseClientService
{
    public const string Version = "v1";

    public CloudGatewayService() : this(new Initializer())
    {
    }

    public CloudGatewayService(Initializer initializer) : base(initializer) => CloudGateway = new CloudGatewayResource(this);

    public override string Name => "cloud_gateway";

    public override string? BasePath => "https://gateway.icloud.com/";

    /// <summary>Gets the CloudGateway resource.</summary>
    public virtual CloudGatewayResource CloudGateway { get; }
}