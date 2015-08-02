using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using ApacheMimeTypes;
using GnomeServer.Logging;
using GnomeServer.ResponseFormatters;

namespace GnomeServer
{
    public class IntegratedWebServer : IWebServer
    {
        // Allows an arbitrary number of bindings by appending a number to the end.
        private const String WebServerHostKey = "webServerHost{0}";

        private static List<String> _logLines;
        private static String _endpoint;

        private WebServer _server;
        private List<IRequestHandler> _requestHandlers;

        /// <summary>
        /// Gets the root endpoint for which the server is configured to service HTTP requests.
        /// </summary>
        public static String Endpoint
        {
            get { return _endpoint; }
        }

        /// <summary>
        /// Gets the full path to the directory where static pages are served from.
        /// </summary>
        public static String GetWebRoot()
        {
            var userDataPath = Configuration.GetUserDataPath();
            var webRoot = Path.Combine(userDataPath, "wwwroot");
            return webRoot;
        }

        /// <summary>
        /// Gets an array containing all currently registered request handlers.
        /// </summary>
        public IRequestHandler[] RequestHandlers
        {
            get { return _requestHandlers.ToArray(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegratedWebServer"/> class.
        /// </summary>
        public IntegratedWebServer()
        {
            // For the entire lifetime of this instance, we'll preseve log messages.
            // After a certain point, it might be worth truncating them, but we'll cross that bridge when we get to it.
            _logLines = new List<String>();

            // We need a place to store all the request handlers that have been registered.
            _requestHandlers = new List<IRequestHandler>();
        }

        public void Start()
        {
            if (_server != null)
            {
                _server.Shutdown();
                _server = null;
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            String version = fvi.FileVersion;
            LogMessage(String.Format("GnomeServer Version {0}", version));

            LogMessage("Initializing Server...");

            List<String> bindings = new List<String>();

            Int32 currentBinding = 1;
            String currentBindingKey = String.Format(WebServerHostKey, currentBinding);
            while (Configuration.HasSetting(currentBindingKey))
            {
                bindings.Add(Configuration.GetString(currentBindingKey));
                currentBinding++;
                currentBindingKey = String.Format(WebServerHostKey, currentBinding);
            }

            // If there are no bindings in the configuration file, we'll need to initialize those values.
            if (bindings.Count == 0)
            {
                const String defaultBinding = "http://localhost:8081/";
                bindings.Add(defaultBinding);

                // If there aren't any bindings, the value of currentBindingKey will never have made it past 1.
                // As a result, we can just use that.
                Configuration.SetString(currentBindingKey, defaultBinding);
                Configuration.SaveSettings();
            }

            // The endpoint used internally should always be the first binding in the configuration.
            // There's no need to use multiple bindings for internal references, we only need a single one.
            _endpoint = bindings.First();

            WebServer ws = new WebServer(HandleRequest, bindings.ToArray());
            _server = ws;
            _server.Initialize();
            LogMessage("Server Initialized.");

            _requestHandlers = new List<IRequestHandler>();

            try
            {
                RegisterHandlers();
            }
            catch (Exception ex)
            {
                LogMessage(ex.ToString(), "Exception");
                throw;
            }
        }

        public void Stop()
        {
            ReleaseServer();

            // TODO: Unregister from events (i.e. ILogAppender.LogMessage)
            _requestHandlers.Clear();

            Configuration.SaveSettings();
        }

        private void ReleaseServer()
        {
            LogMessage("Checking for existing server...");
            if (_server != null)
            {
                LogMessage("Server found; disposing...");
                _server.Shutdown();
                _server = null;
                LogMessage("Server Disposed.");
            }
        }


        /// <summary>;
        /// Handles the specified request.
        /// </summary>
        /// <remarks>
        /// Defers execution to an appropriate request handler, except for requests to the reserved endpoints: <c>~/</c> and <c>~/Log</c>.<br />
        /// Returns a default error message if an appropriate request handler can not be found.
        /// </remarks>
        private void HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            LogMessage(String.Format("{0} {1}", request.HttpMethod, request.RawUrl));

            // There are two reserved endpoints: "/" and "/Log".
            // These take precedence over all other request handlers.
            if (ServiceRoot(request, response))
            {
                return;
            }

            if (ServiceLog(request, response))
            {
                return;
            }

            // Get the request handler associated with the current request.
            var handlers = _requestHandlers.Where(obj => obj.ShouldHandle(request)).ToArray();
            if (handlers.Length > 1)
            {
                LogMessage("Handler Resolution Error: Endpoint did not resolve to a single handler!");
            }
            else if (handlers.Length == 1)
            {
                var handler = handlers[0];
                try
                {
                    IResponseFormatter responseFormatterWriter = handler.Handle(request);
                    responseFormatterWriter.WriteContent(response);

                    return;
                }
                catch (Exception ex)
                {
                    String errorBody = String.Format("<h1>An error has occurred!</h1><pre>{0}</pre>", ex);
                    var tokens = TemplateHelper.GetTokenReplacements("Error", _requestHandlers, errorBody);
                    var template = TemplateHelper.PopulateTemplate("index", tokens);

                    IResponseFormatter errorResponseFormatter = new HtmlResponseFormatter(template);
                    errorResponseFormatter.WriteContent(response);

                    return;
                }
            }

            var wwwroot = GetWebRoot();

            // At this point, we can guarantee that we don't need any game data, so we can safely start a new thread to perform the remaining tasks.
            ServiceFileRequest(wwwroot, request, response);
        }

        private void RegisterHandlers(IEnumerable<Type> handlers)
        {
            if (handlers == null) { return; }

            if (_requestHandlers == null)
            {
                _requestHandlers = new List<IRequestHandler>();
            }

            foreach (var handler in handlers)
            {
                IRequestHandler handlerInstance = null;
                try
                {
                    if (typeof(RequestHandlerBase).IsAssignableFrom(handler))
                    {
                        handlerInstance = (RequestHandlerBase)Activator.CreateInstance(handler);
                    }
                    else
                    {
                        handlerInstance = (IRequestHandler)Activator.CreateInstance(handler);
                    }

                    if (handlerInstance == null)
                    {
                        LogMessage(String.Format("Request Handler ({0}) could not be instantiated!", handler.Name));
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    LogMessage(ex.ToString());
                }

                _requestHandlers.Add(handlerInstance);
                if (handlerInstance is ILogAppender)
                {
                    var logAppender = (handlerInstance as ILogAppender);
                    logAppender.LogMessage += RequestHandlerLogAppender_OnLogMessage;
                }

                LogMessage(String.Format("Added Request Handler: {0}", handler.FullName));
            }
        }

        private void RequestHandlerLogAppender_OnLogMessage(Object sender, LogAppenderEventArgs logAppenderEventArgs)
        {
            var senderTypeName = sender.GetType().Name;
            LogMessage(logAppenderEventArgs.LogLine, senderTypeName);
        }

        /// <summary>
        /// Searches all the assemblies in the current AppDomain, and returns a collection of those that implement the <see cref="IRequestHandler"/> interface.
        /// </summary>
        private static IEnumerable<Type> FindHandlersInLoadedAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var handlers = FetchHandlers(assembly);
                foreach (var handler in handlers)
                {
                    yield return handler;
                }
            }
        }

        private static IEnumerable<Type> FetchHandlers(Assembly assembly)
        {
            //var assemblyName = assembly.GetName().Name;
            //// Skip any assemblies that we don't anticipate finding anything in.
            //if (IgnoredAssemblies.Contains(assemblyName)) { yield break; }

            Type[] types = new Type[0];
            try
            {
                types = assembly.GetTypes();
            }
            catch { }

            foreach (var type in types)
            {
                Boolean isValid = false;
                try
                {
                    isValid = typeof(IRequestHandler).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract;
                }
                catch { }

                if (isValid)
                {
                    yield return type;
                }
            }
        }

        #region Reserved Endpoint Handlers

        /// <summary>
        /// Services requests to <c>~/</c>
        /// </summary>
        private Boolean ServiceRoot(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url.AbsolutePath.ToLower() == "/")
            {
                var wwwroot = GetWebRoot();
                var indexFile = "index.html";
                var absolutePath = Path.Combine(wwwroot, indexFile);
                if (File.Exists(absolutePath))
                {
                    // If an index.html file exists in the root, we won't service root requests any longer.
                    return false;
                }

                List<String> links = new List<String>();
                foreach (var requestHandler in this._requestHandlers.OrderBy(obj => obj.Priority))
                {
                    links.Add(String.Format("<li><a href='{0}'>{0}</a> (Priority: {1})</li>", requestHandler.Name, requestHandler.Priority));
                }

                String body = String.Format("<h1>Gnome Server</h1><ul>{0}</ul>", String.Join("", links.ToArray()));
                var tokens = TemplateHelper.GetTokenReplacements("Home", _requestHandlers, body);
                var template = TemplateHelper.PopulateTemplate("index", tokens);

                IResponseFormatter htmlResponseFormatter = new HtmlResponseFormatter(template);
                htmlResponseFormatter.WriteContent(response);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Services requests to <c>~/Log</c>
        /// </summary>
        private Boolean ServiceLog(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url.AbsolutePath.ToLower() == "/log")
            {
                {
                    String body = String.Format("<h1>Server Log</h1><pre>{0}</pre>", String.Join("", _logLines.ToArray()));
                    var tokens = TemplateHelper.GetTokenReplacements("Log", _requestHandlers, body);
                    var template = TemplateHelper.PopulateTemplate("index", tokens);

                    IResponseFormatter htmlResponseFormatter = new HtmlResponseFormatter(template);
                    htmlResponseFormatter.WriteContent(response);

                    return true;
                }
            }

            return false;
        }

        private static void ServiceFileRequest(String wwwroot, HttpListenerRequest request, HttpListenerResponse response)
        {
            var relativePath = request.Url.AbsolutePath.Substring(1);
            relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());

            // Is the user requesting the default document for a root location?
            if (String.IsNullOrWhiteSpace(relativePath))
            {
                // We'll use "index.html" to indicate this.
                relativePath = "index.html";
            }
            var absolutePath = Path.Combine(wwwroot, relativePath);

            LogMessage(String.Format("ServiceFileRequest: {0} ({1})", request.Url, absolutePath));

            if (File.Exists(absolutePath))
            {
                var extension = Path.GetExtension(absolutePath);
                response.ContentType = Apache.GetMime(extension);
                response.StatusCode = 200; // HTTP 200 - SUCCESS

                // Open file, read bytes into buffer and write them to the output stream.
                using (FileStream fileReader = File.OpenRead(absolutePath))
                {
                    Byte[] buffer = new Byte[4096];
                    Int32 read;
                    while ((read = fileReader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        response.OutputStream.Write(buffer, 0, read);
                    }
                }
            }
            else
            {
                String body = String.Format("No resource is available at the specified filepath: {0}", absolutePath);

                IResponseFormatter notFoundResponseFormatter = new PlainTextResponseFormatter(body, HttpStatusCode.NotFound);
                notFoundResponseFormatter.WriteContent(response);
            }
        }

        /// <summary>
        /// Searches all the assemblies in the current AppDomain for class definitions that implement the <see cref="IRequestHandler"/> interface.  Those classes are instantiated and registered as request handlers.
        /// </summary>
        private void RegisterHandlers()
        {
            IEnumerable<Type> handlers = FindHandlersInLoadedAssemblies();
            RegisterHandlers(handlers);
        }

        #endregion Reserved Endpoint Handlers

        #region Logging

        /// <summary>
        /// Adds a timestamp to the specified message, and appends it to the internal log.
        /// </summary>
        public static void LogMessage(String message, String label = null)
        {
            var dt = DateTime.Now;
            String time = String.Format("{0} {1}", dt.ToShortDateString(), dt.ToShortTimeString());
            String messageWithLabel = String.IsNullOrEmpty(label) ? message : String.Format("{0}: {1}", label, message);
            String line = String.Format("[{0}] {1}{2}", time, messageWithLabel, Environment.NewLine);
            _logLines.Add(line);

            var logPath = Configuration.GetLogFilePath();
            File.AppendAllText(logPath, line);
        }

        /// <summary>
        /// Writes the value of <paramref name="args"/>.<see cref="LogAppenderEventArgs.LogLine"/> to the internal log.
        /// </summary>
        private void ServerOnLogMessage(Object sender, LogAppenderEventArgs args)
        {
            LogMessage(args.LogLine);
        }

        #endregion Logging
    }
}
