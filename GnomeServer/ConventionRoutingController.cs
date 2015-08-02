using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using GnomeServer.Helpers;
using GnomeServer.Routing;

namespace GnomeServer
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public abstract class ConventionRoutingController : RequestHandlerBase
    {
        private String _classRoute;
        private Dictionary<String, MethodInfo> _methodRoutes; 

        public override Boolean ShouldHandle(HttpListenerRequest request)
        {
            CacheRoutes();

            var path = request.Url.AbsolutePath.Substring(1);

            if (path.StartsWith(_classRoute, StringComparison.OrdinalIgnoreCase))
            {
                var remaining = path.Substring(_classRoute.Length);
                if (remaining.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    remaining = remaining.Substring(1);
                }

                String method = request.HttpMethod;
                var matches = _methodRoutes.Where(methodRoute => methodRoute.Key.StartsWith(String.Format("{0}:{1}", method, remaining)));
                if (matches.Any())
                {
                    return true;
                }
            }

            return false;
        }

        public override IResponseFormatter Handle(HttpListenerRequest request)
        {
            // Start at 1 to remove the leading slash from the beginning of the path.
            // Increase offset by _classRoute.Length to remove the class designation from the path.
            var path = request.Url.AbsolutePath.Substring(1 + _classRoute.Length);

            // Even after the path, we remove the *next* leading slash (if exists), between the controller and the action.
            if (path.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
            {
                path = path.Substring(1);
            }

            String actionPath = path;
            Int32 queryStringOffset = actionPath.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
            if (queryStringOffset > 0)
            {
                actionPath = actionPath.Substring(0, queryStringOffset);
            }

            var method = request.HttpMethod;
            var match = _methodRoutes.Single(methodRoute => methodRoute.Key == String.Format("{0}:{1}", method, actionPath));

            var methodInfo = match.Value;
            var parameterInfos = methodInfo.GetParameters();

            // If there are no parameters, we simply won't pass any.
            if (parameterInfos.Length == 0)
            {
                return (IResponseFormatter)methodInfo.Invoke(this, null);
            }
            else
            {
                List<Object> parameters = new List<Object>();

                foreach (var parameterInfo in parameterInfos)
                {
                    String parameterName = parameterInfo.Name;
                    Type parameterType = parameterInfo.ParameterType;

                    if (request.QueryString.HasKey(parameterName))
                    {
                        // TODO: Technically a querystring parameter can be specified multiple times to indicate a collection of values.
                        // However, let's keep it simple for now.

                        String value;
                        String[] values = request.QueryString.GetValues(parameterName);
                        if (values != null)
                        {
                            value = values.SingleOrDefault();
                        }
                        else
                        {
                            value = "";
                        }

                        // In ASP.NET MVC, the model binder is really powerful.
                        // I admit that this one is quite lacking in comparison.
                        if (parameterType == typeof (String))
                        {
                            parameters.Add(value);
                        }
                        else if (parameterType == typeof (Boolean) && value != null)
                        {
                            parameters.Add(Boolean.Parse(value));
                        }
                        else if (parameterType == typeof (Int32) && value != null)
                        {
                            parameters.Add(Int32.Parse(value));
                        }
                        else if (parameterType == typeof (Single) && value != null) // float
                        {
                            parameters.Add(Single.Parse(value));
                        }
                        else if (parameterType == typeof (Double) && value != null)
                        {
                            parameters.Add(Double.Parse(value));
                        }
                    }
                }
                return (IResponseFormatter)methodInfo.Invoke(this, parameters.ToArray());
            }
        }

        private void CacheRoutes()
        {
            // By default, class (and method) definitions are assumed to have an explicitly defined route, unless proven otherwise.
            // If a class is determined to not have an explicit route attribute, use the name of the class minus a trailing "Controller" suffix, if present.
            // If a method is determined to not have an explicit route attribute, use the name of the method, with no modifications.
            if (_classRoute == null || _methodRoutes == null)
            {
                var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
                var type = this.GetType();

                // There can be only one (RouteAttribute per class, that is).
                var classRouteAttribute = type.GetCustomAttributes(typeof(RouteAttribute), true).Cast<RouteAttribute>().SingleOrDefault();

                if (classRouteAttribute == null)
                {
                    var className = type.Name;
                    const String suffixToRemove = "Controller";
                    if (className.EndsWith(suffixToRemove, StringComparison.InvariantCultureIgnoreCase))
                    {
                        className = className.Substring(0, className.Length - suffixToRemove.Length);
                    }

                    _classRoute = className;
                }
                else
                {
                    // TODO: Should probably sanitize these and throw exceptions if an invalid value is specified.
                    _classRoute = classRouteAttribute.Route;
                }

                var methodRoutes = new Dictionary<String, MethodInfo>();

                // Find all methods that return IResponseFormatter and were declared within the class (i.e. no overrides from the base class.)
                var methods = type.GetMethods(bindingFlags).Where(method => typeof(IResponseFormatter).IsAssignableFrom(method.ReturnType) && method.DeclaringType == type).ToArray();
                OnLogMessage(String.Format("Found {0} methods", methods.Length));
                foreach (var methodInfo in methods)
                {
                    var methodRouteAttribute = methodInfo.GetCustomAttributes(typeof(RouteAttribute), true).Cast<RouteAttribute>().SingleOrDefault();
                    var methodMethodAttribute = methodInfo.GetCustomAttributes(typeof(IHttpMethodAttribute), true).Cast<IHttpMethodAttribute>().SingleOrDefault();

                    StringBuilder methodRouteBuilder = new StringBuilder();

                    if (methodMethodAttribute == null)
                    {
                        // When no method is explicitly set, assume GET.
                        methodRouteBuilder.Append("GET:");
                    }
                    else
                    {
                        // Otherwise, use the method that was set.
                        methodRouteBuilder.Append(String.Format("{0}:", methodMethodAttribute.Method));
                    }

                    if (methodRouteAttribute == null)
                    {
                        methodRouteBuilder.Append(methodInfo.Name);
                    }
                    else
                    {
                        // TODO: Should probably sanitize these and throw exceptions if an invalid value is specified.
                        methodRouteBuilder.Append(methodRouteAttribute.Route);
                    }
                    methodRoutes.Add(methodRouteBuilder.ToString(), methodInfo);
                }
                _methodRoutes = methodRoutes;
            }
        }
    }
}
