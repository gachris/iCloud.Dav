using iCloud.Dav.Core.Log;
using System;

namespace iCloud.Dav.Core;

public static class ApplicationContext
{
    private static ILogger _logger;

    /// <summary>Returns the logger used within this application context.</summary>
    /// <remarks>It creates a <see cref="NullLogger" /> if no logger was registered previously</remarks>
    public static ILogger Logger => _logger ??= new NullLogger();

    /// <summary>Registers a logger with this application context.</summary>
    /// <exception cref="T:System.InvalidOperationException">Thrown if a logger was already registered.</exception>
    public static void RegisterLogger(ILogger loggerToRegister)
    {
        if (_logger != null && _logger is not NullLogger)
            throw new InvalidOperationException("A logger was already registered with this context.");
        _logger = loggerToRegister;
    }
}
