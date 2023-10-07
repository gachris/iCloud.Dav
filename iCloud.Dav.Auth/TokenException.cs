using iCloud.Dav.Core;
using System;

namespace iCloud.Dav.Auth;

/// <summary>
/// Token response exception which is thrown in case of receiving a token error when an authorization code or an
/// access token is expected.
/// </summary>
public class TokenException : Exception
{
    /// <summary>The error information.</summary>
    public ErrorResponse Error { get; private set; }

    /// <summary>Constructs a new token exception.</summary>
    public TokenException(string message) : base(message)
    {
    }

    /// <summary>Constructs a new token exception from the given error.</summary>
    public TokenException(ErrorResponse error) : base(error.ToString()) => Error = error;
}