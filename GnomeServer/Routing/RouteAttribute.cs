using System;

namespace GnomeServer.Routing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RouteAttribute : Attribute
    {
        public readonly String Route;

        public RouteAttribute(String route)
        {
            this.Route = route;
        }
    }
}
