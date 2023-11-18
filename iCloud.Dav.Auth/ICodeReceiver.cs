using System.Net;

namespace iCloud.Dav.Auth;

/// <summary>Verification code receiver.</summary>
public interface ICodeReceiver
{
    /// <summary>Receives the authorization code.</summary>
    /// <param name="networkCredential">The NetworkCredential request.</param>
    /// <returns>The authorization code response</returns>
    string ReceiveCode(NetworkCredential networkCredential);
}