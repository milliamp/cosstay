using CosStay.Model;
using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site.Controllers
{    
    public class BaseController : Controller
    {
        public ClaimsIdentity Identity
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication.User.Identity as ClaimsIdentity;
            }
        }

        protected T ValidateDetails<T>(DbSet<T> set, int id, string name) where T : NamedEntity
        {
            var instance = set.Find(id);
            if (instance == null)
                throw new HttpException(404, id + " not found");

            if (name != SafeUri(instance.Name))
                Response.RedirectToRoutePermanent(new
                {
                    controller = this.RouteData.Values["controller"],
                    action = "Details",
                    id = id,
                    name = SafeUri(instance.Name)
                });

            return instance;
        }

        public string SafeUri(string toSanitise)
        {
            var safeString = "-_()!";
            return new string(toSanitise
                .Replace(" - ", "-")
                .Replace(" ", "-")
                .Replace(": ", "-")
                .Replace(":", "-")
                .Replace(", ", " ")
                .Replace("--", "-")
                .Where(c => Char.IsLetterOrDigit(c) || safeString.Contains(c))
                .ToArray())
                .ToLowerInvariant();
        }

        public string FacebookAccessToken
        {
            get
            {
                var c = Identity.FindFirst("FacebookAccessToken");
                if (c != null)
                    return c.Value;
                return null;
            }
        }

        public FacebookClient FacebookClient
        {
            get
            {
                var fb = new FacebookClient(FacebookAccessToken);
                fb.AppId = ConfigurationManager.AppSettings["FacebookAppId"];
                fb.AppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"];
                return fb;
            }
        }

        public int? FacebookUserId
        {
            get
            {

                var c = Identity.FindFirst(string.Format("urn:facebook:{0}", "id"));
                if (c != null)
                    return int.Parse(c.Value);
                return null;
            }
        }

    }
}