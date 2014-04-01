using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace CosStay.Site
{
    public class CmsUrlConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            using (var db = new CosStayContext())
            {
                if (values[parameterName] != null)
                {
                    var permalink = values[parameterName].ToString();
                    return db.ContentPages.Any(p => p.Uri == permalink);
                }
                return false;
            }
        }
    }

}