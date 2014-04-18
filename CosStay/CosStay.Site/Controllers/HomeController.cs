using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*using (var db = new CosStayContext())
            {
                CosStay.Model.Utilities.SeedData(db);
            } */

            return View();
        }

        public ActionResult About()
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