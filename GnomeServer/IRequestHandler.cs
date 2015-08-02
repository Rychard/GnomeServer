using System;
using System.Net;

namespace GnomeServer
{
    /// <summary>
    /// Represents a handler for servicing requests received by the web server.
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// Gets the priority of this request handler.  A request will be handled by the request handler with the lowest priority.
        /// </summary>
        Int32 Priority { get; }

        /// <summary>
        /// Gets the display name of this request handler.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Returns a value that indicates whether this handler is capable of servicing the given request.
        /// </summary>
        Boolean ShouldHandle(HttpListenerRequest request);

        /// <summary>
        /// Handles the specified request.  The method should not close the stream.
        /// </summary>
        IResponseFormatter Handle(HttpListenerRequest request);
    }
}
