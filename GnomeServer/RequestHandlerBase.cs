using System;
using System.Net;
using GnomeServer.Logging;
using GnomeServer.ResponseFormatters;

namespace GnomeServer
{
    public abstract class RequestHandlerBase : IRequestHandler, ILogAppender
    {
        #region ILogAppender Implementation

        public event EventHandler<LogAppenderEventArgs> LogMessage;

        protected void OnLogMessage(String message)
        {
            var handler = LogMessage;
            if (handler != null)
            {
                handler(this, new LogAppenderEventArgs(message));
            }
        }

        #endregion ILogAppender Implementation

        /// <summary>
        /// Gets the display name of this request handler.
        /// </summary>
        public virtual String Name
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// Gets the priority of this request handler.  A request will be handled by the request handler with the lowest priority.
        /// </summary>
        public Int32 Priority { get; set; }

        /// <summary>
        /// Returns a value that indicates whether this handler is capable of servicing the given request.
        /// </summary>
        public abstract Boolean ShouldHandle(HttpListenerRequest request);

        /// <summary>
        /// Handles the specified request.  The method should not close the stream.
        /// </summary>
        public abstract IResponseFormatter Handle(HttpListenerRequest request);

        /// <summary>
        /// Returns a response in JSON format.
        /// </summary>
        protected IResponseFormatter JsonResponse<T>(T content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new JsonResponseFormatter<T>(content, statusCode);
        }

        /// <summary>
        /// Returns a response in HTML format.
        /// </summary>
        protected IResponseFormatter HtmlResponse(String content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new HtmlResponseFormatter(content, statusCode);
        }

        /// <summary>
        /// Returns a response in plain text format.
        /// </summary>
        protected IResponseFormatter PlainTextResponse(String content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new PlainTextResponseFormatter(content, statusCode);
        }

        /// <summary>
        /// Returns a blank response.
        /// </summary>
        protected IResponseFormatter BlankResponse(HttpStatusCode statusCode)
        {
            return new BlankResponseFormatter(statusCode);
        }
    }
}
