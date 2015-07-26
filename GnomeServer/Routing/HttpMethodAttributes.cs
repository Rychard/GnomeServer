using System;

namespace GnomeServer.Routing
{
    public interface IHttpMethodAttribute {  }

    /// <summary>
    /// Specifies that an action supports the GET HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HttpGetAttribute : Attribute, IHttpMethodAttribute { }

    /// <summary>
    /// Specifies that an action supports the POST HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HttpPostAttribute : Attribute, IHttpMethodAttribute { }

    /// <summary>
    /// Specifies that an action supports the PUT HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HttpPutAttribute : Attribute, IHttpMethodAttribute { }

    /// <summary>
    /// Specifies that an action supports the DELETE HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HttpDeleteAttribute : Attribute, IHttpMethodAttribute { }
}
