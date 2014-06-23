using CosStay.Core.Services;
using CosStay.Model;
using CosStay.Site.Models;
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
        protected IAuthorizationService _auth;
        protected BaseController(IEntityStore entityStore, IAuthorizationService authorizationService)
        {
            _es = entityStore;
            _auth = authorizationService;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            /*if (Request.IsAuthenticated && await GetCurrentUserAsync() == null)
                HttpContext.GetOwinContext().Authentication.SignOut();*/
        }

        public ClaimsIdentity Identity
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication.User.Identity as ClaimsIdentity;
            }
        }

        public async Task SetSharedViewParameters()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                ViewBag.CurrentUserName = user.Name;
                ViewBag.CurrentUserEmail = user.Email;
            }
            catch {
                if (HttpContext.Request.IsAuthenticated)
                    HttpContext.GetOwinContext().Authentication.SignOut();
                // Swallow
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            AsyncInline.Run(async () => await SetSharedViewParameters());
            base.OnActionExecuted(filterContext);
        }

        private User _currentUser = null;
        public async Task<User> GetCurrentUserAsync()
        {
            if (!Request.IsAuthenticated)
            {
                _currentUser = null;
                return null;
            }
                
            if (_currentUser != null)
                return _currentUser;

            _currentUser = await _es.GetAsync<User>(Identity.GetUserId());
                return _currentUser;
            
        }

        protected async Task<T> ValidateDetailsAsync<T>(IEntityStore es, int id, string name) where T : NamedEntity
        {
            var instance = await _es.GetAsync<T>(id);
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


        protected async Task DenyIfNotAuthorizedAsync<TEntity>(ActionType actionType) where TEntity : class
        {
            
            if (!_auth.IsAuthorizedTo<TEntity>(await GetCurrentUserAsync(), actionType, default(TEntity)))
                throw new HttpException(403, "Access Denied");
        }

        protected async Task DenyIfNotAuthorizedAsync<TEntity>(ActionType actionType, TEntity entity) where TEntity : class
        {
            if (!_auth.IsAuthorizedTo(await GetCurrentUserAsync(), actionType, entity))
                throw new HttpException(403, "Access Denied");
        }

        public string SafeUri(string toSanitise)
        {
            return UtilityMethods.SafeUri(toSanitise);
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