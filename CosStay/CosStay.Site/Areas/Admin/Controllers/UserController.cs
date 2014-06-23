using CosStay.Core.Services;
using CosStay.Model;
using CosStay.Site.Areas.Admin.Models;
using CosStay.Site.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IEntityStore entityStore, IAuthorizationService authorizationService)
            : base(entityStore, authorizationService)
        {
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            var users = _es.GetAll<User>().Where(u => !u.IsDeleted);
            return View(users);
        }

        // GET: Admin/User/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var user = await _es.GetAsync<User>(id.ToString());
            if (user == null)
                throw new HttpException(404, "No user by that ID");
            var audits = await _es.GetAll<Audit>()
                .Where(a => a.InitiatingUser.Id == user.Id)
                .OrderByDescending(a => a.Id)
                .Take(50)
                .ToAsyncList();

            foreach (var audit in audits)
            {
                try
                {
                    var de = JsonConvert.DeserializeObject(audit.Data);
                    audit.Data = JsonConvert.SerializeObject(de, Formatting.Indented);
                }
                catch (JsonReaderException)
                {
                }
            }

            var vm = new UserAuditViewModel()
            {
                User = user,
                Audits = audits.ToArray()
            };
            return View(vm);
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
