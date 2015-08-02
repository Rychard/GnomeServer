﻿using System;
using System.Net;
using System.Threading;

namespace GnomeServer
{
    public class WebServer
    {
        private readonly Object Locker = new Object();
        private readonly HttpListener _listener = new HttpListener();
        private readonly Action<HttpListenerRequest, HttpListenerResponse> _responderMethod;

        public WebServer(String[] prefixes, Action<HttpListenerRequest, HttpListenerResponse> method)
        {
            if (!HttpListener.IsSupported) { throw new NotSupportedException("This wouldn't happen if you upgraded your operating system more than once a decade."); }

            // URI prefixes are required, for example:
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0) { throw new ArgumentException("prefixes"); }

            // A responder method is required
            if (method == null) { throw new ArgumentException("method"); }

            foreach (String s in prefixes)
            {
                _listener.Prefixes.Add(s);
            }

            _responderMethod = method;
            _listener.Start();
        }

        public WebServer(Action<HttpListenerRequest, HttpListenerResponse> method, params String[] prefixes) : this(prefixes, method)
        {
        }

        public void Initialize()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(RequestHandlerCallback, _listener.GetContext());
                    }
                }
                catch { } // Suppress exceptions.
            });
        }

        public void Shutdown()
        {
            _listener.Stop();
            _listener.Close();
        }

        private void RequestHandlerCallback(Object context)
        {
            var ctx = context as HttpListenerContext;
            try
            {
                if (ctx != null)
                {
                    // Service only a single request at a time.
                    lock (Locker)
                    {
                        var request = ctx.Request;
                        var response = ctx.Response;

                        // Allow accessing pages from pages hosted from another local web-server, such as IIS, for instance.
                        response.AddHeader("Access-Control-Allow-Origin", "http://localhost");

                        _responderMethod(request, response);
                    }
                }
            }
            catch { } // Suppress any exceptions.
            finally
            {
                if (ctx != null)
                {
                    // Ensure that the stream is never left open.
                    ctx.Response.OutputStream.Close();
                }
            }
        }
    }
}
