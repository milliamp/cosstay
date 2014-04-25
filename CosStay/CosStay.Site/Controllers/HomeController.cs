using CosStay.Core.Services;
using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IEntityStore entityStore, IAuthorizationService authorizationService):base(entityStore, authorizationService)
        {

        }
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public  ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}