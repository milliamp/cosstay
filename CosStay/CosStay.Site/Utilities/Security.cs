using CosStay.Core.Services;
using CosStay.Model;
using CosStay.Site.App_Start;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site
{
    public static class Security
    {
        public static bool IsAccessibleToUser<TEntity>(IPrincipal principal, string actionAuthorize, string controllerAuthorize, TEntity entity) where TEntity:class
        {
            // Find Controller
            Assembly assembly = Assembly.GetExecutingAssembly();
            GetControllerType(controllerAuthorize);
            Type controllerType = GetControllerType(controllerAuthorize);
            //var controller = (IController)Activator.CreateInstance(controllerType);

            // Find Attributes
            ArrayList controllerAuthorizeAttributes = new ArrayList(controllerType.GetCustomAttributes(typeof(AuthorizeAttribute), true));
            ArrayList actionAuthorizeAttributes = new ArrayList();
            ArrayList actionTypeAttributes = new ArrayList();
            MethodInfo[] methods = controllerType.GetMethods();
            foreach (MethodInfo method in methods)
            {
                object[] attributes = method.GetCustomAttributes(typeof(ActionNameAttribute), true);
                if ((attributes.Length == 0 && method.Name == actionAuthorize) || (attributes.Length > 0 && ((ActionNameAttribute)attributes[0]).Name == actionAuthorize))
                {
                    actionAuthorizeAttributes.AddRange(method.GetCustomAttributes(typeof(AuthorizeAttribute), true));
                    actionTypeAttributes.AddRange(method.GetCustomAttributes(typeof(ActionTypeAttribute), true));
                }
            }
            if (controllerAuthorizeAttributes.Count == 0 && actionAuthorizeAttributes.Count == 0)
                return true;

            // sort out privileges
            ActionType actionType = ActionType.Read;
            string roles = "";
            string users = "";
            if (controllerAuthorizeAttributes.Count > 0)
            {
                AuthorizeAttribute attribute = controllerAuthorizeAttributes[0] as AuthorizeAttribute;
                roles += attribute.Roles;
                users += attribute.Users;
            }
            if (actionAuthorizeAttributes.Count > 0)
            {
                AuthorizeAttribute attribute = actionAuthorizeAttributes[0] as AuthorizeAttribute;
                roles += attribute.Roles;
                users += attribute.Users;
            }

            if (actionTypeAttributes.Count > 0)
            {
                ActionTypeAttribute attribute = actionTypeAttributes[0] as ActionTypeAttribute;
                actionType = attribute.ActionType;
            }

            if (!principal.Identity.IsAuthenticated)
                return false;

            IAuthorizationService service = NinjectWebCommon.Kernel.GetService(typeof(IAuthorizationService)) as IAuthorizationService;
            //if (service.IsAuthorizedTo(actionType, entity))
                return service.IsAuthorizedTo(actionType, entity);
            /*
            if (string.IsNullOrEmpty(roles) && string.IsNullOrEmpty(users) && principal.Identity.IsAuthenticated)
                return true;

            string[] roleArray = roles.Split(',');
            string[] usersArray = users.Split(',');
            foreach (string role in roleArray)
            {
                if (role == "*" || principal.IsInRole(role))
                    return true;
            }
            foreach (string user in usersArray)
            {
                if (user == "*" || (principal.Identity.Name == user))
                    return true;                    
            }
            return false;*/
        }

        public static Type GetControllerType(string controllerName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(Controller).IsAssignableFrom(type) && (type.Name.ToUpper() == (controllerName.ToUpper() + "Controller".ToUpper())))
                {
                    return type;
                }
            }
            return null;
        }
    }
}