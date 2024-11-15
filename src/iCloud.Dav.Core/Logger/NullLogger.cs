namespace iCloud.Dav.Core.Logger;

/// <summary>
/// Provides a logger implementation that discards all log messages and does not log any information.
/// </summary>
public class NullLogger : ILogger
{
    /// <summary>
    /// Gets a value indicating whether debug output is logged or not.
    /// </summary>
    public bool IsDebugEnabled => false;

    /// <summary>
    /// Returns a logger which will be associated with the specified type.
    /// </summary>
    /// <param name="type">Type to which this logger belongs.</param>
    /// <returns>A type-associated logger.</returns>
    public ILogger ForType(Type type) => new NullLogger();

    /// <summary>
    /// Returns a logger which will be associated with the specified type.
    /// </summary>
    /// <typeparam name="T">Type to which this logger belongs.</typeparam>
    /// <returns>A type-associated logger.</returns>
    public ILogger ForType<T>() => new NullLogger();

    /// <summary>
    /// Logs an info message. This implementation does nothing.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    public void Info(string message, params object[] formatArgs)
    {
    }

    /// <summary>
    /// Logs a warning message. This implementation does nothing.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    public void Warning(string message, params object[] formatArgs)
    {
    }

    /// <summary>
    /// Logs a debug message. This implementation does nothing.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    public void Debug(string message, params object[] formatArgs)
    {
    }

    /// <summary>
    /// Logs an error message resulting from an exception. This implementation does nothing.
    /// </summary>
    /// <param name="exception">The exception that caused the error.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    public void Error(Exception exception, string message, params object[] formatArgs)
    {
    }

    /// <summary>
    /// Logs an error message. This implementation does nothing.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    public void Error(string message, params object[] formatArgs)
    {
    }
}