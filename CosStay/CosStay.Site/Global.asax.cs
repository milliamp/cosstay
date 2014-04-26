using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CosStay.Site
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMaps();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //You don't want to redirect on posts, or images/css/js
            bool isGet = HttpContext.Current.Request.RequestType.ToLowerInvariant().Contains("get");
            if (isGet && HttpContext.Current.Request.Url.AbsolutePath.Contains('.') == false)
            {
                string lowercaseURL = (Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.Url.AbsolutePath);
                if (Regex.IsMatch(lowercaseURL, @"[A-Z]"))
                {
                    //You don't want to change casing on query strings
                    lowercaseURL = lowercaseURL.ToLower() + HttpContext.Current.Request.Url.Query;

                    Response.Clear();
                    Response.Status = "301 Moved Permanently";
                    Response.AddHeader("Location", lowercaseURL);
                    Response.End();
                }
            }
        }

        /*protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            Server.ClearError();
            
            int statusCode = 0;

            if (lastError.GetType() == typeof(HttpException))
            {
                statusCode = ((HttpException)lastError).GetHttpCode();
            }
            else
            {
                // Not an HTTP related error so this is a problem in our code, set status to
                // 500 (internal server error)
                statusCode = 500;
            }

            HttpContextWrapper contextWrapper = new HttpContextWrapper(this.Context);

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Page");
            routeData.Values.Add("action", "ErrorContent");
            routeData.Values.Add("statusCode", statusCode);
            routeData.Values.Add("exception", lastError);
            routeData.Values.Add("isAjaxRequest", contextWrapper.Request.IsAjaxRequest());

            IController controller = new PageController();

            RequestContext requestContext = new RequestContext(contextWrapper, routeData);

            controller.Execute(requestContext);
            Response.End();
        }*/


    }
}
