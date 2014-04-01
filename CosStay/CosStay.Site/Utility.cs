﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace CosStay.Site
{
public class RouteInfo
    {
        public RouteInfo(RouteData data)
        {
            RouteData = data;
        }

        public RouteInfo(Uri uri, string applicationPath)
        {
            RouteData = RouteTable.Routes.GetRouteData(new InternalHttpContext(uri, applicationPath));
        }

        public RouteData RouteData { get; private set; }

        private class InternalHttpContext : HttpContextBase
        {
            private readonly HttpRequestBase _request;

            public InternalHttpContext(Uri uri, string applicationPath) : base()
            {
                _request = new InternalRequestContext(uri, applicationPath);
            }

            public override HttpRequestBase Request { get { return _request; } }
        }

        private class InternalRequestContext : HttpRequestBase
        {
            private readonly string _appRelativePath;
            private readonly string _pathInfo;

            public InternalRequestContext(Uri uri, string applicationPath) : base()
            {
                _pathInfo = ""; //uri.Query; (this was causing problems, see comments - Stuart)

                if (String.IsNullOrEmpty(applicationPath) || !uri.AbsolutePath.StartsWith(applicationPath, StringComparison.OrdinalIgnoreCase))
                    _appRelativePath = uri.AbsolutePath.Substring(applicationPath.Length);
                else
                    _appRelativePath = uri.AbsolutePath;
            }

            public override string AppRelativeCurrentExecutionFilePath { get { return String.Concat("~", _appRelativePath); } }
            public override string PathInfo { get { return _pathInfo; } }
        }
    }

    /// <summary>
    /// Extension methods for the Uri class
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Indicates whether the supplied url matches the specified controller and action values based on the MVC routing table defined in global.asax.
        /// </summary>
        /// <param name="uri">A Uri object containing the url to evaluate</param>
        /// <param name="controllerName">The name of the controller class to match</param>
        /// <param name="actionName">The name of the action method to match</param>
        /// <returns>True if the supplied url is mapped to the supplied controller class and action method, false otherwise.</returns>
        public static bool IsRouteMatch(this Uri uri, string controllerName, string actionName)
        {
            RouteInfo routeInfo = new RouteInfo(uri, HttpContext.Current.Request.ApplicationPath);
            return (routeInfo.RouteData.Values["controller"].ToString() == controllerName && routeInfo.RouteData.Values["action"].ToString() == actionName);
        }
    }

}