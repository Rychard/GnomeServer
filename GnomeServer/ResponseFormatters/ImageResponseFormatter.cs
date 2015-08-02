using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace GnomeServer.ResponseFormatters
{
    internal class ImageResponseFormatter : IResponseFormatter
    {
        private readonly Bitmap _content;
        private readonly HttpStatusCode _statusCode;

        public ImageResponseFormatter(Bitmap content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _content = content;
            _statusCode = statusCode;
        }

        public override void WriteContent(HttpListenerResponse response)
        {
            response.StatusCode = (Int32)_statusCode;
            response.ContentType = "image/png";
            // TODO: Determine whether we need to set the ContentLength64 property.
            _content.Save(response.OutputStream, ImageFormat.Png);

            // Make sure we dispose of the underlying bitmap.
            _content.Dispose();
        }
    }
}
