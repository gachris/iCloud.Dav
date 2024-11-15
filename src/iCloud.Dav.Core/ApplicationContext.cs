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
    public static ILogger Logger
    {
        get
        {
            if (_logger == null)
            {
                _logger = new NullLogger();
            }

            return _logger;
        }
    }

    /// <summary>
    /// Registers a logger with this application context.
    /// </summary>
    /// <param name="loggerToRegister">The logger instance to register.</param>
    /// <exception cref="InvalidOperationException">Thrown if a logger was already registered.</exception>
    public static void RegisterLogger(ILogger loggerToRegister)
    {
        if (_logger is not null and not NullLogger)
        {
            throw new InvalidOperationException("A logger was already registered with this context.");
        }

        _logger = loggerToRegister;
    }
}