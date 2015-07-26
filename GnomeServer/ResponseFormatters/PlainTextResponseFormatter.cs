﻿using System;
using System.Net;
using System.Text;

namespace GnomeServer.ResponseFormatters
{
    internal class PlainTextResponseFormatter : IResponseFormatter
    {
        private readonly String _content;
        private readonly HttpStatusCode _statusCode;

        public PlainTextResponseFormatter(String content, HttpStatusCode statusCode)
        {
            _content = content;
            _statusCode = statusCode;
        }

        public override void WriteContent(HttpListenerResponse response)
        {
            byte[] buf = Encoding.UTF8.GetBytes(_content);

            response.StatusCode = (int)_statusCode;
            response.ContentType = "text/plain";
            response.ContentLength64 = buf.Length;
            response.OutputStream.Write(buf, 0, buf.Length);
        }
    }
}
