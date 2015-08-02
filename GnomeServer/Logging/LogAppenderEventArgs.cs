using System;

namespace GnomeServer.Logging
{
    public class LogAppenderEventArgs : EventArgs
    {
        public String LogLine { get; private set; }

        public LogAppenderEventArgs(String logLine)
        {
            LogLine = logLine;
        }
    }
}
