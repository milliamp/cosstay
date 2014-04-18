using CosStay.Core.Services;
using CosStay.Model;
using Facebook;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IEntityStore _es;
        protected BaseController(IEntityStore entityStore)
        {
            _es = entityStore;
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Request.IsAuthenticated && CurrentUser == null)
                HttpContext.GetOwinContext().Authentication.SignOut();
        }
        public ClaimsIdentity Identity
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication.User.Identity as ClaimsIdentity;
            }
        }

        private User _currentUser = null;
        public User CurrentUser
        {
            get
            {
                if (!Request.IsAuthenticated)
                {
                    _currentUser = null;
                    return null;
                }
                
                if (_currentUser != null)
                    return _currentUser;

                    _currentUser = _es.Get<User>(Identity.GetUserId());
                    return _currentUser;
                
            }

        }

        protected T ValidateDetails<T>(IEntityStore es, int id, string name) where T : NamedEntity
        {
            var instance = es.Get<T>(id);
            if (instance == null)
                throw new HttpException(404, id + " not found");

            if (name != SafeUri(instance.Name))
                Response.RedirectToRoutePermanent(new
                {
                    controller = this.RouteData.Values["controller"],
                    action = this.RouteData.Values["action"],
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

        public int? FacebookUserId { 
            get
            {
                var claim = Identity.Claims.FirstOrDefault(c => c.Type == "urn:facebook:id");
                if (claim == null)
                    return null;
                return int.Parse(claim.Value);
            }
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult { 
                Data = data, 
                ContentEncoding = contentEncoding,
                ContentType = contentType,
                JsonRequestBehavior = behavior
            };
        }
    }
}