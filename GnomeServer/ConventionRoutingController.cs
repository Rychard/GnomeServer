﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using GnomeServer.Helpers;
using GnomeServer.Routing;

namespace GnomeServer
{
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

                var matches = _methodRoutes.Where(methodRoute => methodRoute.Key.StartsWith(remaining));
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
            if (path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(1);
            }

            String actionPath = path;
            int queryStringOffset = actionPath.IndexOf("?", StringComparison.OrdinalIgnoreCase);
            if (queryStringOffset > 0)
            {
                actionPath = actionPath.Substring(0, queryStringOffset);
            }
            var match = _methodRoutes.Single(methodRoute => methodRoute.Key == actionPath);

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
                        var value = request.QueryString.GetValues(parameterName).SingleOrDefault();

                        // In ASP.NET MVC, the model binder is really powerful.
                        // I admit that this one is quite lacking in comparison.
                        if (parameterType == typeof (String))
                        {
                            parameters.Add(value);
                        }
                        else if (parameterType == typeof (Boolean))
                        {
                            parameters.Add(Boolean.Parse(value));
                        }
                        else if (parameterType == typeof (Int32))
                        {
                            parameters.Add(Int32.Parse(value));
                        }
                        else if (parameterType == typeof (Single)) // float
                        {
                            parameters.Add(Single.Parse(value));
                        }
                        else if (parameterType == typeof (Double)) // float
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
                    if (className.EndsWith(suffixToRemove, StringComparison.OrdinalIgnoreCase))
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
                var methods = type.GetMethods(bindingFlags).Where(method => typeof(IResponseFormatter).IsAssignableFrom(method.ReturnType)).ToArray();
                OnLogMessage(String.Format("Found {0} methods", methods.Length));
                foreach (var methodInfo in methods)
                {
                    OnLogMessage("Checking: " + methodInfo.Name);
                    var methodRouteAttribute = methodInfo.GetCustomAttributes(typeof(RouteAttribute), true).Cast<RouteAttribute>().SingleOrDefault();
                    
                    String methodRoute;
                    if (methodRouteAttribute == null)
                    {
                        methodRoute = methodInfo.Name;
                        
                    }
                    else
                    {
                        // TODO: Should probably sanitize these and throw exceptions if an invalid value is specified.
                        methodRoute = methodRouteAttribute.Route;
                    }
                    methodRoutes.Add(methodRoute, methodInfo);
                }
                _methodRoutes = methodRoutes;
            }
        }
    }
}
