using System.Linq;
using System.Net;
using System.Web.Mvc;
using CosStay.Model;
using CosStay.Site.Models;
using AutoMapper;
using System;
using System.Web;
using CosStay.Core.Services;
using System.Threading.Tasks;

namespace CosStay.Site.Controllers
{
    [RoutePrefix("accomodation")]
    [Route("{action=index}")]
    public class AccomodationVenueController : BaseController
    {
        private IAccomodationVenueService _accomodationVenueService;
        public AccomodationVenueController(IAccomodationVenueService accomodationVenueService, IEntityStore entityStore, IAuthorizationService authorizationService)
            : base(entityStore, authorizationService)
        {
            _accomodationVenueService = accomodationVenueService;
        }

        // GET: /AccomodationVenue/
        public async Task<ActionResult> Index()
        {
            var list = await _es.GetAll<AccomodationVenue>().IncludePaths(v => v.Owner).ToAsyncList();
            list = list.Where(v => _auth.IsAuthorizedTo(ActionType.Read, v)).ToList();
            return View(list);
        }

        // GET: /AccomodationVenue/Details/5
        [Route("{id:int}/{name?}")]
        [ActionType(ActionType.Read)]
        public async Task<ActionResult> Details(int id, string name)
        {
            var instance = await ValidateDetailsAsync<AccomodationVenue>(_es, id, name);
            await DenyIfNotAuthorizedAsync(ActionType.Read, instance);
            return View(instance);
        }

        // GET: /AccomodationVenue/Create
        [Route("set-up/")]
        [Authorize]
        [ActionType(ActionType.Create)]
        public async Task<ActionResult> Create()
        {
            await DenyIfNotAuthorizedAsync<AccomodationVenue>(ActionType.Create);
            return View();
        }

        // POST: /AccomodationVenue/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("set-up/")]
        [ActionType(ActionType.Create)]
        public async Task<ActionResult> Create([Bind(Include="Name,Address,AllowsBedSharing,AllowsMixedRooms")] AccomodationVenue accomodationvenue)
        {
            await DenyIfNotAuthorizedAsync<AccomodationVenue>(ActionType.Create);
            if (ModelState.IsValid)
            {
                var currentUser = await GetCurrentUserAsync();
                
                _es.Add(accomodationvenue);
                
                accomodationvenue.LatLng = new LatLng();
                accomodationvenue.Owner = await _es.GetAsync<User>(currentUser.Id);
                accomodationvenue.DateAdded = DateTimeOffset.Now;
                
                //db.Users.Attach(accomodationvenue.Owner);
                //db.SaveChanges();
                await _es.SaveAsync();
                return RedirectToAction("Index");
            }

            return View(accomodationvenue);
        }

        // GET: /AccomodationVenue/Edit/5
        [Route("edit/{id:int}")]
        [Authorize]
        [ActionType(ActionType.Update)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccomodationVenue accomodationvenue = await _es.GetAsync<AccomodationVenue>(id.Value);
            if (accomodationvenue == null)
            {
                return HttpNotFound();
            }

            await DenyIfNotAuthorizedAsync(ActionType.Update, accomodationvenue);
            var vm = GetViewModel(accomodationvenue);

            return View(vm);
        }

        // POST: /AccomodationVenue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Route("edit/{id:int}")]
        [Authorize]
        [ActionType(ActionType.Update)]
        public async Task<JsonResult> Edit(int id, AccomodationVenueViewModel accomodationVenueViewModel)
        {
            if (ModelState.IsValid)
            {
                AccomodationVenue instance = await _es.GetAsync<AccomodationVenue>(id);

                await DenyIfNotAuthorizedAsync(ActionType.Update, instance);

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
                await _accomodationVenueService.UpdateVenueAsync(dtoInstance);
            }

            AccomodationVenue vmInstance = await _es.GetAsync<AccomodationVenue>(id);
            return Json(GetViewModel(vmInstance));
        }

        // GET: /AccomodationVenue/Delete/5
        [Route("delete/{id:int}")]
        [Authorize]
        [ActionType(ActionType.Delete)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccomodationVenue accomodationvenue = await _es.GetAsync<AccomodationVenue>(id.Value);
            if (accomodationvenue == null)
            {
                return HttpNotFound();
            }

            await DenyIfNotAuthorizedAsync(ActionType.Delete, accomodationvenue);
            return View(accomodationvenue);
        }

        // POST: /AccomodationVenue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete/{id:int}")]
        [Authorize]
        [ActionType(ActionType.Delete)]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AccomodationVenue accomodationvenue = await _es.GetAsync<AccomodationVenue>(id);

            if (accomodationvenue == null)
            {
                return HttpNotFound();
            }

            await DenyIfNotAuthorizedAsync(ActionType.Delete, accomodationvenue);
            
            _es.Delete(accomodationvenue);
            await _es.SaveAsync();
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
            vm.Rooms = vm.Rooms.Where(r => !r.IsDeleted).ToList();


            vm.AvailableBedSizes = _es.GetAll<BedSize>().ToList();
            vm.AvailableBedTypes = _es.GetAll<BedType>().ToList();
            return vm;
        }
    }
}
