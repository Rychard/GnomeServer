using System;
using System.Net;

namespace GnomeServer.ResponseFormatters
{
    internal class BlankResponseFormatter : IResponseFormatter
    {
        private readonly HttpStatusCode _statusCode;

        public BlankResponseFormatter(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        public override void WriteContent(HttpListenerResponse response)
        { 
            response.StatusCode = (Int32)_statusCode;
        }
    }
}
