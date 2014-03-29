using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CosStay.Model;

namespace CosStay.Site.Controllers
{
    [RoutePrefix("accomodation")]
    [Route("{action=index}")]
    public class AccomodationVenueController : BaseController
    {
        private CosStayContext db = new CosStayContext();

        // GET: /AccomodationVenue/
        public ActionResult Index()
        {
            return View(db.AccomodationVenues.ToList());
        }

        // GET: /AccomodationVenue/Details/5
        [Route("{id:int}/{name?}")]
        public ActionResult Details(int id, string name)
        {
            var instance = ValidateDetails(db.AccomodationVenues, id, name);

            return View(instance);
        }

        // GET: /AccomodationVenue/Create
        [Route("set-up/")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /AccomodationVenue/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("set-up/")]
        public ActionResult Create([Bind(Include="Name,Address,AllowsBedSharing,AllowsMixedRooms")] AccomodationVenue accomodationvenue)
        {
            if (ModelState.IsValid)
            {
                db.AccomodationVenues.Add(accomodationvenue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(accomodationvenue);
        }

        // GET: /AccomodationVenue/Edit/5
        [Route("edit/{id:int}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccomodationVenue accomodationvenue = db.AccomodationVenues.Find(id);
            if (accomodationvenue == null)
            {
                return HttpNotFound();
            }
            return View(accomodationvenue);
        }

        // POST: /AccomodationVenue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{id:int}")]
        public ActionResult Edit([Bind(Include="AccomodationVenueId,Name,Address,DateAdded,AllowsBedSharing,AllowsMixedRooms")] AccomodationVenue accomodationvenue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accomodationvenue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accomodationvenue);
        }

        // GET: /AccomodationVenue/Delete/5
        [Route("delete/{id:int}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccomodationVenue accomodationvenue = db.AccomodationVenues.Find(id);
            if (accomodationvenue == null)
            {
                return HttpNotFound();
            }
            return View(accomodationvenue);
        }

        // POST: /AccomodationVenue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete/{id:int}")]
        public ActionResult DeleteConfirmed(int id)
        {
            AccomodationVenue accomodationvenue = db.AccomodationVenues.Find(id);
            db.AccomodationVenues.Remove(accomodationvenue);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
