using System.Linq;
using System.Net;
using System.Web.Mvc;
using CosStay.Model;
using CosStay.Site.Models;
using AutoMapper;
using System;
using System.Web;
using CosStay.Core.Services;

namespace CosStay.Site.Controllers
{
    [RoutePrefix("accomodation")]
    [Route("{action=index}")]
    public class AccomodationVenueController : BaseController
    {
        private IEntityStore _es;

        private IAccomodationVenueService _accomodationVenueService;
        public AccomodationVenueController(IAccomodationVenueService accomodationVenueService, IEntityStore entityStore)
        {
            _accomodationVenueService = accomodationVenueService;
            _es = entityStore;
        }

        // GET: /AccomodationVenue/
        public ActionResult Index()
        {
            return View(_es.GetAll<AccomodationVenue>().ToList());
        }

        // GET: /AccomodationVenue/Details/5
        [Route("{id:int}/{name?}")]
        public ActionResult Details(int id, string name)
        {
            var instance = ValidateDetails<AccomodationVenue>(_es, id, name);
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
        [Authorize]
        [Route("set-up/")]
        public ActionResult Create([Bind(Include="Name,Address,AllowsBedSharing,AllowsMixedRooms")] AccomodationVenue accomodationvenue)
        {
            if (ModelState.IsValid)
            {
                var currentUser = CurrentUser;
                
                _es.Add(accomodationvenue);
                
                accomodationvenue.LatLng = new LatLng();
                accomodationvenue.Owner = _es.Get<User>(currentUser.Id);
                accomodationvenue.DateAdded = DateTimeOffset.Now;
                
                //db.Users.Attach(accomodationvenue.Owner);
                //db.SaveChanges();
                _es.Save();
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
            AccomodationVenue accomodationvenue = _es.Get<AccomodationVenue>(id.Value);
            if (accomodationvenue == null)
            {
                return HttpNotFound();
            }

            var vm = GetViewModel(accomodationvenue);

            return View(vm);
        }

        // POST: /AccomodationVenue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Route("edit/{id:int}")]
        public JsonResult Edit(int id, AccomodationVenueViewModel accomodationVenueViewModel)
        {
            if (ModelState.IsValid)
            {
                AccomodationVenue instance = _es.Get<AccomodationVenue>(id);
                /*if (instance.Owner.Id != CurrentUser.Id)
                {
                    ModelState.AddModelError("Access Denied", new HttpException(403, "Access Denied"));
                    return View(accomodationVenueViewModel);
                }*/
                var dtoInstance = Mapper.Map<AccomodationVenueViewModel, AccomodationVenue>(accomodationVenueViewModel);
                
                
                //instance.Rooms = dtoInstance.Rooms;
                

                /*// TODO: Check Authorisation based on venue
                instance.Address = accomodationVenueViewModel.Address;
                instance.AllowsBedSharing = accomodationVenueViewModel.AllowsBedSharing;
                instance.AllowsMixedRooms = accomodationVenueViewModel.AllowsMixedRooms;
                instance.Features = accomodationVenueViewModel.Features;
                instance.LatLng = accomodationVenueViewModel.LatLng ?? new LatLng(0,0);
                instance.Location = accomodationVenueViewModel.Location;
                instance.Photos = accomodationVenueViewModel.Photos;
                // TODO: Rooms merge
                //instance.Rooms = accomodationVenueViewModel.Rooms.sel;*/
                _accomodationVenueService.UpdateVenue(dtoInstance);
            }


            AccomodationVenue vmInstance = _es.Get<AccomodationVenue>(id);
            return Json(GetViewModel(vmInstance));
        }

        // GET: /AccomodationVenue/Delete/5
        [Route("delete/{id:int}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccomodationVenue accomodationvenue = _es.Get<AccomodationVenue>(id.Value);
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
            AccomodationVenue accomodationvenue = _es.Get<AccomodationVenue>(id);
            //TODO:Delete
            //db.AccomodationVenues.Remove(accomodationvenue);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _es.Dispose();
            }
            base.Dispose(disposing);
        }

        public AccomodationVenueViewModel GetViewModel(AccomodationVenue venue)
        {
            var vm = Mapper.Map<AccomodationVenueViewModel>(venue);
            
            vm.AvailableBedSizes = _es.GetAll<BedSize>().ToList();
            vm.AvailableBedTypes = _es.GetAll<BedType>().ToList();
            return vm;
        }
    }
}
