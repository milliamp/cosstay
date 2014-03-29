using CosStay.Model;
using CosStay.Site.Models;
using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site.Controllers
{
    public class PageController : Controller
    {
        //
        // GET: /Page/
        public ActionResult Content(string permalink, int? version)
        {
            using (var db = new CosStayContext())
            {
                var page = db.ContentPages.First(p => p.Uri == permalink);
                /*if (string.IsNullOrWhiteSpace(version) && User.Identity.isAdmin)
                    Response.RedirectToRoutePermanent(new {

                    });*/
               
                IEnumerable<ContentPageVersion> pageVersions = page.Versions;

                if (version != null)
                {
                    pageVersions = pageVersions.Where(pv => pv.Version == version);
                }
                else
                {
                    pageVersions = pageVersions
                        .Where(pv => pv.Status == ContentPageVersionStatus.Published)
                        .OrderByDescending(pv => pv.Version);
                }

                var theVersion = pageVersions.First();

                var md = new Markdown(new MarkdownOptions() {
                    AutoHyperlink = true,
                });
                var vm = new ContentPageViewModel()
                {
                    CreatedDate = theVersion.CreatedDate,
                    HtmlContent = md.Transform(theVersion.MarkdownContent),
                    MarkdownContent = theVersion.MarkdownContent,
                    PublishDate = theVersion.PublishDate,
                    Status = theVersion.Status,
                    Uri = page.Uri,
                    Title = theVersion.Title

                };

                return View("Content", vm);
            }
        }

        public ActionResult ErrorContent(int statusCode, Exception error, bool isAjaxRequest)
        {
            Response.StatusCode = statusCode;
            switch (statusCode)
            {
                case 404:
                    return Content("404error", null);
                case 500:
                    return Content("500error", null);
                default:
                    return Content("500error", null);
            }
        }

	}
}