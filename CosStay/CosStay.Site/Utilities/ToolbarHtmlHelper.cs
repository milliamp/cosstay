using CosStay.Core.Services;
using CosStay.Site.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CosStay.Site
{
    public static class ToolbarHtmlHelper
    {
        public static MvcHtmlString Toolbar<TClass>(this HtmlHelper<TClass> helper, dynamic model)
        {
            var actions = GetActions(helper.ViewContext.Controller.GetType());

            var controller = helper.ViewContext.Controller as Controller;
            var authService = NinjectWebCommon.Kernel.GetService(typeof(IAuthorizationService)) as IAuthorizationService;
            var controllerName= helper.ViewContext.RouteData.Values["controller"].ToString();
                
            var resultString = "";
            foreach (var action in actions)
            {
                if (!Security.IsAccessibleToUser(HttpContext.Current.User, action.Name, controllerName, model))
                    continue;

                if (!action.GetParameters().Any(p => p.Name == "id"))
                    continue;
                
                var routeValues = new RouteValueDictionary();
                
                routeValues.Add("id", model.Id);
                if (model.Name != null && action.GetParameters().Any(p => p.Name == "name"))
                    routeValues.Add("name", UtilityMethods.SafeUri(model.Name));
                var url = controller.Url.Action(action.Name, routeValues);

                if (url == helper.ViewContext.RequestContext.HttpContext.Request.Path)
                    continue;

                var style = "primary";
                if (action.GetCustomAttributes<ActionTypeAttribute>().Any(t => t.ActionType == ActionType.Delete))
                    style = "danger";

                var actionName = action.Name;
                actionName = Regex.Replace(actionName, @"(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", @" ${a}");


                resultString += @"<a href=""" + url + @""" class=""btn btn-" + style + @""">" + actionName + "</a>\n";
            }
            
            return new MvcHtmlString(resultString);
        }

        private static MethodInfo[] GetActions(Type controllerType)
        {
            
            var actionCandidates = controllerType.GetMethods(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);
            var actionMethods = actionCandidates
                .Where(a => typeof(ActionResult).IsAssignableFrom(a.ReturnType)
                || typeof(Task<ActionResult>).IsAssignableFrom(a.ReturnType))
                .Where(a => !a.GetCustomAttributes(typeof(HttpPostAttribute), false).Any())
                .Where(a => !a.GetCustomAttributes(typeof(HttpPutAttribute), false).Any())
                .Where(a => !a.GetCustomAttributes(typeof(HttpDeleteAttribute), false).Any());


            return actionMethods.ToArray();
        }
        
    }
}