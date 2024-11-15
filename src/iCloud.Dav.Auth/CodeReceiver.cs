using System.Net;
using iCloud.Dav.Core;
using iCloud.Dav.Core.Logger;

namespace iCloud.Dav.Auth;

/// <summary>Verification code receiver that generate authorization verification code.</summary>
public class CodeReceiver : ICodeReceiver
{
    private static readonly ILogger Logger = ApplicationContext.Logger.ForType<CodeReceiver>();

    /// <summary>Receives the authorization code.</summary>
    /// <param name="networkCredential">The NetworkCredential request.</param>
    /// <returns>The authorization code response</returns>
    public string ReceiveCode(NetworkCredential networkCredential)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(string.Concat(networkCredential.UserName, ":", networkCredential.Password));
        var basicAuthorization = Convert.ToBase64String(plainTextBytes);
        return basicAuthorization;
    }
}