using System.Net;

namespace GnomeServer
{
    public abstract class IResponseFormatter
    {
        public abstract void WriteContent(HttpListenerResponse response);
    }
}
