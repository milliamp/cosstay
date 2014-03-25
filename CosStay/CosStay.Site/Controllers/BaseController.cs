using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
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