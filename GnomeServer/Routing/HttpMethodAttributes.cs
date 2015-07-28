using System;

namespace GnomeServer.Routing
{
    public interface IHttpMethodAttribute
    {
        String Method { get; }
    }

    /// <summary>
    /// Specifies that an action supports the GET HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HttpGetAttribute : Attribute, IHttpMethodAttribute
    {
        public String Method { get { return "GET"; } }
    }

    /// <summary>
    /// Specifies that an action supports the POST HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HttpPostAttribute : Attribute, IHttpMethodAttribute
    {
        public String Method { get { return "POST"; } }
    }

    /// <summary>
    /// Specifies that an action supports the PUT HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HttpPutAttribute : Attribute, IHttpMethodAttribute
    {
        public String Method { get { return "PUT"; } }
    }

    /// <summary>
    /// Specifies that an action supports the DELETE HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HttpDeleteAttribute : Attribute, IHttpMethodAttribute
    {
        public String Method { get { return "DELETE"; } }
    }
}
