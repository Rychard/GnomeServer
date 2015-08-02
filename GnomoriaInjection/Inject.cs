using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Game;

// TODO: Isolate the server to its own AppDomain to allow for loading/unloading assemblies at runtime.
// TODO: Determine how to best perform communication between these appdomains.
using GnomeServer;

namespace GnomoriaInjection
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Inject
    {
        private static readonly Object _lockObject = new Object();
        private static IWebServer _webServer;
        private static Boolean _stopping;

        public static void Hook()
        {
            // Prevent the server from starting up again once it is terminated.
            if (_stopping) { return; }

            lock (_lockObject)
            {
                if (_webServer == null)
                {
                    try
                    {
                        _webServer = new IntegratedWebServer();
                        _webServer.Start();
                        GnomanEmpire.Instance.Exiting += OnExiting;
                    }
                    catch (Exception ex)
                    {
                        LogException(ex);

                        // It's important that we still throw an exception here.
                        // Exceptions thrown at this point are undoubtedly on the GnomanEmpire.Instance property.
                        // So, exit here to avoid the risk of bricking someone's game save.
                        throw;
                    }
                }
            }
        }

        private static void LogException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ex.ToString());

            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var errorLogFileName = "errorlog.txt";
            var errorLogPath = Path.Combine(desktop, errorLogFileName);
            File.WriteAllText(errorLogPath, sb.ToString());
        }

        private static void OnExiting(Object sender, EventArgs args)
        {
            var server = _webServer;
            if (server != null)
            {
                _stopping = true;
                server.Stop();
            }
        }
    }
}
