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
using System.Collections.Generic;

namespace CosStay.Site.Controllers
{
    [RoutePrefix("accomodation")]
    [Route("{action=index}")]
    public class AccomodationVenueController : BaseController
    {
        private IAccomodationVenueService _accomodationVenueService;
        private ITravelService _travelService;
        private IDateTimeService _dateTimeService;
        private ILocationService _locationService;

        public AccomodationVenueController(IAccomodationVenueService accomodationVenueService,
            IEntityStore entityStore,
            IAuthorizationService authorizationService,
            ITravelService travelService,
            IDateTimeService dateTimeService,
            ILocationService locationService)
            : base(entityStore, authorizationService)
        {
            _accomodationVenueService = accomodationVenueService;
            _travelService = travelService;
            _dateTimeService = dateTimeService;
            _locationService = locationService;
        }

        // GET: /AccomodationVenue/
        public async Task<ActionResult> Index(string q, int l = 0, int e = 0, int start = 0, int limit = int.MaxValue, bool json = false)
        {
            var list = new List<AccomodationVenueViewModel>();
            var totalResults = 0;

            if (Request.IsAjaxRequest() || json)
            {
                IQueryable<VenueAvailabilitySearchResult> query;

                // TODO: Need to expose viewmodel with availability in it.
                if (e > 0)
                    query = await _accomodationVenueService.AvailableVenuesQueryAsync(e);
                else
                    query = _es.GetAll<AccomodationVenue>().IncludePaths(v => v.Owner).Select(v => new VenueAvailabilitySearchResult() {
                        Venue = v
                    });
                
                if (!string.IsNullOrWhiteSpace(q))
                {
                    query = query.Where(vsr =>
                        vsr.Venue.Name.Contains(q)
                        || vsr.Venue.PublicAddress.Contains(q)
                        || vsr.Venue.Location.Name.Contains(q)
                        || vsr.Venue.Rooms.Any(r => r.Description.Contains(q))
                        || vsr.Venue.Rooms.Any(r => r.Name.Contains(q)));
                }
                if (l > 0)
                    query = query.Where(vsr => vsr.Venue.Location.Id == l);

                var dataList = await query.ToAsyncList();
                dataList = dataList.Where(v => _auth.IsAuthorizedTo(ActionType.Read, v)).ToList();
                totalResults = dataList.Count;
                list = dataList.Skip(start)
                    .Take(limit)
                    .Select(vsr => GetSearchViewModel(vsr))
                    .ToList();

                return Json(new
                {
                    Items = list,
                    TotalCount = totalResults
                });
            }
            return View(_es.GetAll<Location>().ToList().Select(loc => Mapper.Map<LocationViewModel>(loc)));
        }

        // GET: /AccomodationVenue/Details/5
        [Route("{id:int}/{name?}")]
        [ActionType(ActionType.Read)]
        public async Task<ActionResult> Details(int id, string name)
        {
            var instance = await ValidateDetailsAsync<AccomodationVenue>(_es, id, name);
            DenyIfNotAuthorizedAsync(ActionType.Read, instance);

            var fullInstance = _es.GetAll<AccomodationVenue>()
                .IncludePaths(
                    v => v.CoverImage,
                    v => v.Residents,
                    v => v.Rooms,
                    v => v.Location,
                    v => v.Rooms.Select(r => r.Beds),
                    v => v.Rooms.Select(r => r.Features),
                    v => v.Rooms.Select(r => r.Photos),
                    v => v.Rooms.Select(r => r.Beds.Select(b => b.BedSize)),
                    v => v.Rooms.Select(r => r.Beds.Select(b => b.BedType)),
                    v => v.Rooms.Select(r => r.Beds.Select(b => b.Photos)))
                .Single(v => v.Id == id);
            var vm = GetExtendedViewModel(fullInstance);

            // TODO: Move to EventService
            var eventsQuery = _es.GetAll<EventInstance>()
                .IncludePaths(
                    e => e.Venue,
                    e => e.Venue.Location,
                    e => e.Photos,
                    e => e.Event,
                    e => e.Event.Photos
                )
                .Where(e => e.StartDate > _dateTimeService.Now);
            
            if (instance.Location != null)
                eventsQuery = eventsQuery.Where(e => e.Venue.Location.Id == instance.Location.Id);

            var events = await eventsQuery.ToAsyncList();

            vm.TravelInfo = new Dictionary<EventInstance, TravelInfo>();
            vm.AvailabilityViewModel = new Dictionary<EventInstance, AccomodationRoomAvailabilityViewModel>();
            foreach (var eventInstance in events)
            {
                vm.AvailabilityViewModel[eventInstance] = await GetAvailabilityViewModel(id, eventInstance.Id);
                if (eventInstance.Venue == null || !eventInstance.Venue.LatLng.HasValue || !vm.LatLng.HasValue)
                    continue;
                vm.TravelInfo[eventInstance] = _travelService.CalculateTravelInfo(vm.LatLng, eventInstance.Venue.LatLng);
            }
            return View(vm);
        }

        // GET: /AccomodationVenue/Create
        [Route("set-up/")]
        [Authorize]
        [ActionType(ActionType.Create)]
        public async Task<ActionResult> Create()
        {
            DenyIfNotAuthorizedAsync<AccomodationVenue>(ActionType.Create);
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
        public async Task<ActionResult> Create([Bind(Include = "Name,Address,AllowsBedSharing,AllowsMixedRooms")] AccomodationVenue accomodationvenue)
        {
            DenyIfNotAuthorizedAsync<AccomodationVenue>(ActionType.Create);
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

            DenyIfNotAuthorizedAsync(ActionType.Update, accomodationvenue);
            var vm = GetExtendedViewModel(accomodationvenue);

            return View(vm);
        }

        [Route("setup/{id:int}")]
        [Authorize]
        [ActionType(ActionType.Update)]
        public async Task<ActionResult> Setup(int? id)
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

            DenyIfNotAuthorizedAsync(ActionType.Update, accomodationvenue);
            var vm = GetExtendedViewModel(accomodationvenue);

            return View(vm);
        }

        // POST: /AccomodationVenue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [Route("setup/{id:int}")]
        [Authorize]
        [ActionType(ActionType.Update)]
        public async Task<JsonResult> Setup(int id, AccomodationVenueViewModel accomodationVenueViewModel)
        {
            if (ModelState.IsValid)
            {
                AccomodationVenue instance = await _es.GetAsync<AccomodationVenue>(id);

                DenyIfNotAuthorizedAsync(ActionType.Update, instance);

                var dtoInstance = Mapper.Map<AccomodationVenueViewModel, AccomodationVenue>(accomodationVenueViewModel);

                await _accomodationVenueService.UpdateVenueAsync(dtoInstance);
            }

            AccomodationVenue vmInstance = await _es.GetAsync<AccomodationVenue>(id);
            return Json(GetExtendedViewModel(vmInstance));
        }

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

                DenyIfNotAuthorizedAsync(ActionType.Update, instance);

                var dtoInstance = Mapper.Map<AccomodationVenueViewModel, AccomodationVenue>(accomodationVenueViewModel);

                //await _accomodationVenueService.UpdateBedAvailability(dtoInstance);
            }

            AccomodationVenue vmInstance = await _es.GetAsync<AccomodationVenue>(id);
            return Json(GetExtendedViewModel(vmInstance));
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

            DenyIfNotAuthorizedAsync(ActionType.Delete, accomodationvenue);
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

            DenyIfNotAuthorizedAsync(ActionType.Delete, accomodationvenue);

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

        // TODO: Can we get this async to stop 2 concurrent blocking calls?
        public AccomodationVenueViewModel GetExtendedViewModel(AccomodationVenue venue)
        {
            var vm = Mapper.Map<AccomodationVenueViewModel>(venue);
            vm.Rooms = vm.Rooms.Where(r => !r.IsDeleted).ToList();

            if (vm.LatLng.HasValue)
                vm.LatLng = _locationService.Approximate(vm.LatLng);
            vm.AvailableBedSizes = _es.GetAll<BedSize>().ToList();
            vm.AvailableBedTypes = _es.GetAll<BedType>().ToList();
            return vm;
        }

        public AccomodationVenueViewModel GetBasicViewModel(AccomodationVenue venue)
        {
            var vm = Mapper.Map<AccomodationVenueViewModel>(venue);
            vm.Rooms = null;
            if (!string.IsNullOrWhiteSpace(venue.PublicAddress))
                vm.Address = venue.PublicAddress;
            if (vm.LatLng.HasValue)
                vm.LatLng = _locationService.Approximate(vm.LatLng);
            return vm;
        }
        public async Task<AccomodationRoomAvailabilityViewModel> GetAvailabilityViewModel(int venueId, int eventInstanceId)
        {
            var availableBeds = await _accomodationVenueService.AvailableBedsQueryAsync(venueId, eventInstanceId);
            var venueInstance = await _es.GetAsync<AccomodationVenue>(venueId);
            var beds = new List<BedAvailabilityViewModel>();
            var list = await availableBeds.ToAsyncList();
            foreach (var night in list)
            {
                var bed = Mapper.Map<BedAvailabilityViewModel>(night.Bed);
                bed.Nights = new List<Tuple<DateTime, BedStatus>>();
                foreach (var bedNight in night.Nights)
                {
                    bed.Nights.Add(new Tuple<DateTime,BedStatus>(bedNight.Night.DateTime, bedNight.BedStatus));
                }

                beds.Add(bed);
            }

            var nights = new List<DateTime>();
            if (list.Any())
            {
                var startDate = list.First().StartDate;
                var endDate = list.First().EndDate.AddDays(1);
                var cursorDate = startDate.Date;
                while (cursorDate <= endDate)
                {
                    nights.Add(cursorDate);
                    cursorDate = cursorDate.AddDays(1);
                }
            }
            var vm = new AccomodationRoomAvailabilityViewModel()
            {
                Id = venueId,
                Nights = nights,
                BedAvailability = beds,
                Beds = Mapper.Map<List<BedViewModel>>(venueInstance.Rooms.SelectMany(r => r.Beds))
            };

            return vm;
        }
        public AccomodationVenueViewModel GetSearchViewModel(VenueAvailabilitySearchResult searchResult)
        {
            var vm = Mapper.Map<AccomodationVenueViewModel>(searchResult.Venue);
            vm.Rooms = null;
            if (!string.IsNullOrWhiteSpace(searchResult.Venue.PublicAddress))
                vm.Address = searchResult.Venue.PublicAddress;
            if (vm.LatLng.HasValue)
                vm.LatLng = _locationService.Approximate(vm.LatLng);

            if (searchResult.Nights != null)
            {
                var groups = searchResult.Nights.GroupBy(n => n.Bed).Select(g => new { Bed = g.Key, Nights = g.Select(z => z.Night) });

                var bedAvailabilityGroups = new List<Tuple<List<Bed>, List<DateTimeOffset>>>();
                foreach (var bed in groups)
                {
                    var existing = bedAvailabilityGroups.FirstOrDefault(g => g.Item2.SequenceEqual(bed.Nights));
                    if (existing == null)
                    {
                        existing = new Tuple<List<Bed>, List<DateTimeOffset>>(new List<Bed>(), bed.Nights.OrderBy(n => n).ToList());
                        bedAvailabilityGroups.Add(existing);
                    }
                    existing.Item1.Add(bed.Bed);
                }

                var bedAvailabilityStrings = new List<string>();
                foreach (var bedGroups in bedAvailabilityGroups)
                {
                    bedAvailabilityStrings.Add(string.Format("{0} bed{1} available {2}",
                        bedGroups.Item1.Count(),
                        bedGroups.Item1.Count() != 1 ? "s" : "",
                        string.Join(", ", bedGroups.Item2.Select(n => n.ToString("dddd")))));
                }
                vm.BedAvailability = string.Join(". ", bedAvailabilityStrings);
            }
            return vm;
        }
    }
}
