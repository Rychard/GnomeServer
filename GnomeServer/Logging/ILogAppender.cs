using System;

namespace GnomeServer.Logging
{
    public interface ILogAppender
    {
        event EventHandler<LogAppenderEventArgs> LogMessage;
    }
}
