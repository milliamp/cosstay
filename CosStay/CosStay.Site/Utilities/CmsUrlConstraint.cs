using CosStay.Core.Services;
using CosStay.Model;
using CosStay.Site.App_Start;
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
            var _es = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IEntityStore)) as IEntityStore;

                if (values[parameterName] != null)
                {
                    var permalink = values[parameterName].ToString();
                    return _es.GetAll<ContentPage>().Any(p => p.Uri == permalink);
                }
                return false;
        }
    }

}