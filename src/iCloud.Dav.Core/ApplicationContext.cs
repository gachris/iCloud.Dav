using iCloud.Dav.Core.Logger;

namespace iCloud.Dav.Core;

/// <summary>
/// The application context class that manages the logger instance.
/// </summary>
public static class ApplicationContext
{
    private static ILogger _logger;

    /// <summary>
    /// Gets the logger instance used within this application context. Creates a new <see cref="NullLogger"/> if no logger was registered previously.
    /// </summary>
    public static ILogger Logger => _logger ??= new NullLogger();

    /// <summary>
    /// Registers a logger with this application context.
    /// </summary>
    /// <param name="logger">The logger instance to register.</param>
    /// <exception cref="InvalidOperationException">Thrown if a logger was already registered.</exception>
    public static void RegisterLogger(ILogger logger)
    {
        if (_logger is not null and not NullLogger)
        {
            throw new InvalidOperationException("A logger was already registered with this context.");
        }

        _logger = logger;
    }
}