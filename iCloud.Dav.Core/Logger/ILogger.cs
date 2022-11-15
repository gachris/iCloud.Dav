using System;

namespace iCloud.Dav.Core.Logger;

public interface ILogger
{
    /// <summary>Gets an indication whether debug output is logged or not.</summary>
    bool IsDebugEnabled { get; }

    /// <summary>Returns a logger which will be associated with the specified type.</summary>
    /// <param name="type">Type to which this logger belongs.</param>
    /// <returns>A type-associated logger.</returns>
    ILogger ForType(Type type);

    /// <summary>Returns a logger which will be associated with the specified type.</summary>
    /// <returns>A type-associated logger.</returns>
    ILogger ForType<T>();

    /// <summary>Logs an info message.</summary>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    void Info(string message, params object[] formatArgs);

    /// <summary>Logs a warning.</summary>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    void Warning(string message, params object[] formatArgs);

    /// <summary>Logs a debug message.</summary>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    void Debug(string message, params object[] formatArgs);

    /// <summary>Logs an error message resulting from an exception.</summary>
    /// <param name="exception"></param>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    void Error(Exception? exception, string message, params object[] formatArgs);

    /// <summary>Logs an error message.</summary>
    /// <param name="message">The message to log.</param>
    /// <param name="formatArgs">String.Format arguments (if applicable).</param>
    void Error(string message, params object[] formatArgs);
}
