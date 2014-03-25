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
            using (var db = new CosStayContext())
            {
                CosStay.Model.Utilities.SeedData(db);
                // Create and save a new Blog 


                // Display all Blogs from the database 
                var query = from b in db.Locations
                            orderby b.Name
                            select b;
                
                //("All locations in the database:");
                foreach (var item in query)
                {
                    Response.Write(item.Name);
                }
                

            } 

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