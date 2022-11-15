using System;

namespace iCloud.Dav.Core.Logger;

public class NullLogger : ILogger
{
    public bool IsDebugEnabled => false;

    public ILogger ForType(Type type) => new NullLogger();

    public ILogger ForType<T>() => new NullLogger();

    public void Info(string message, params object[] formatArgs)
    {
    }

    public void Warning(string message, params object[] formatArgs)
    {
    }

    public void Debug(string message, params object[] formatArgs)
    {
    }

    public void Error(Exception? exception, string message, params object[] formatArgs)
    {
    }

    public void Error(string message, params object[] formatArgs)
    {
    }
}
