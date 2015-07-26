using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace GnomeServer.ResponseFormatters
{
    internal class JsonResponseFormatter<T> : IResponseFormatter
    {
        private readonly T _content;
        private readonly HttpStatusCode _statusCode;

        public JsonResponseFormatter(T content, HttpStatusCode statusCode)
        {
            _content = content;
            _statusCode = statusCode;
        }

        public override void WriteContent(HttpListenerResponse response)
        {
            var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(_content, Formatting.Indented);
            byte[] buf = Encoding.UTF8.GetBytes(serializedData);

            response.StatusCode = (int)_statusCode;
            response.ContentType = "text/json";
            response.ContentLength64 = buf.Length;
            response.OutputStream.Write(buf, 0, buf.Length);
        }
    }
}
