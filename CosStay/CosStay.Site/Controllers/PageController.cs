using CosStay.Core.Services;
using CosStay.Model;
using CosStay.Site.Models;
using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site.Controllers
{
    public class PageController : BaseController
    {
        public PageController(IEntityStore entityStore, IAuthorizationService authorizationService)
            : base(entityStore, authorizationService)
        {

        }
        //
        // GET: /Page/
        public async Task<ActionResult> Content(string permalink, int? version, bool edit = false)
        {
            var page = _es.GetAll<ContentPage>().First(p => p.Uri == permalink);
            if (edit && _auth.IsAuthorizedTo<ContentPage>(ActionType.Update, page))
                return View("Edit", await GetContentAsync(page, version));
            return View(await GetContentAsync(page, version));
        }

        public async Task<ActionResult> ErrorContent(int statusCode, Exception error, bool isAjaxRequest)
        {
            Response.StatusCode = statusCode;
            switch (statusCode)
            {
                case 404:
                    return View("Content", await GetContentAsync("404error", null));
                case 500:
                    var exceptionDetail = error.Message + "<hr/>";
                    var insideError = error;
                    while (insideError != null)
                    {
                        exceptionDetail += insideError.StackTrace + "<br/><br/>";
                        insideError = insideError.InnerException;
                    }
                    exceptionDetail += error.TargetSite;
                    return View("Content", await GetContentAsync("500error", null, exceptionDetail));
                default:
                    return View("Content", await GetContentAsync("500error", null));
            }
        }
        
        public async Task<ContentPageViewModel> GetContentAsync(string permalink, int? version, string extraText = "")
        {
            return await GetContentAsync(_es.GetAll<ContentPage>().First(p => p.Uri == permalink), version, extraText);
        }
        public async Task<ContentPageViewModel> GetContentAsync(ContentPage page, int? version, string extraText = "")
        {
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

            var theVersion = pageVersions.FirstOrDefault();
            
            await SetSharedViewParameters();

            if (theVersion == null)
                return new ContentPageViewModel() {
                    CreatedDate = DateTimeOffset.Now,
                    HtmlContent = "<em>Page has no content</em>",
                    PublishDate = DateTimeOffset.Now,
                    Status = ContentPageVersionStatus.Draft,
                    Title = page.Uri,
                    Uri = page.Uri
                };

            var md = new Markdown(new MarkdownOptions()
            {
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