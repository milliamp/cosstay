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
            return View(GetContent(permalink, version));
        }

        public ActionResult ErrorContent(int statusCode, Exception error, bool isAjaxRequest)
        {
            Response.StatusCode = statusCode;
            switch (statusCode)
            {
                case 404:
                    return View("Content", GetContent("404error", null));
                case 500:
                    var exceptionDetail = error.Message + "<hr/>";
                    var insideError = error;
                    while (insideError != null)
                    {
                        exceptionDetail += insideError.StackTrace + "<br/><br/>";
                        insideError = insideError.InnerException;
                    }
                    exceptionDetail += error.TargetSite;
                    return View("Content", GetContent("500error", null, exceptionDetail));
                default:
                    return View("Content", GetContent("500error", null));
            }
        }

        public ContentPageViewModel GetContent(string permalink, int? version, string extraText = "")
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
                    HtmlContent = md.Transform(theVersion.MarkdownContent) + extraText,
                    MarkdownContent = theVersion.MarkdownContent,
                    PublishDate = theVersion.PublishDate,
                    Status = theVersion.Status,
                    Uri = page.Uri,
                    Title = theVersion.Title

                };

                return vm;
            }

        }
	}
}