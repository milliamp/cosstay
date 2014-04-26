using CosStay.Model;
using Facebook;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.IO;
using CosStay.Site.Models;
using CosStay.Core.Services;
using System.Threading.Tasks;

namespace CosStay.Site.Controllers
{
    [RoutePrefix("events")]
    [Route("{action=index}")]
    public class EventController : BaseController
    {
        public EventController(IAppFacebookService appFacebookService, IUserFacebookService userFacebookService, IEntityStore entityStore, IAuthorizationService authorizationService, ILocationService locationService, IVenueService venueService)
            : base(entityStore, authorizationService)
        {
            _appFacebookService = appFacebookService;
            _userFacebookService = userFacebookService;
            _locationService = locationService;
            _venueService = venueService;
        }

        private IAppFacebookService _appFacebookService;
        public FacebookClient AppFacebookClient
        {
            get
            {
                return _appFacebookService.Client;
            }

        }

        private ILocationService _locationService;
        private IVenueService _venueService;

        public IUserFacebookService _userFacebookService { get; set; }
        public FacebookClient UserFacebookClient
        {
            get
            {
                return _userFacebookService.Client;
            }
        }

        public async Task<ActionResult> Index()
        {

            if (_es.GetAll<EventInstance>().Count() == 0)
            {
                var event1 = new Event()
                {
                    Name = "Hamilton Armageddon"

                };

                var event2 = new Event()
                {
                    Name = "Auckland Armageddon"

                };
                var instance1 = new EventInstance()
                {
                    Name = "Hamilton Armageddon 2014",
                    StartDate = new DateTime(2014, 05, 14),
                    EndDate = new DateTime(2014, 05, 20),
                    Description = "ALL THE GEDDON",
                    FacebookEventId = "581423075239944",
                    DateUpdated = DateTimeOffset.MinValue,
                    Url = "http://milliamp.org"
                };
                var instance2 = new EventInstance()
                {
                    Name = "Auckgeddon",
                    Description = "AUCKLAND",
                    FacebookEventId = "761346600558963",
                    StartDate = new DateTime(2014, 05, 14),
                    EndDate = new DateTime(2014, 05, 20),
                    DateUpdated = DateTimeOffset.MinValue
                };
                instance1.Event = event1;

                instance2.Event = event2;
                _es.Add(event1);
                _es.Add(instance1);
                _es.Add(event2);
                _es.Add(instance2);
                await _es.SaveAsync();
            }
            return View(_es.GetAll<EventInstance>().OrderBy(ei => ei.StartDate).ToArray());

        }

        [Route("{id:int}/{name?}")]
        [ActionType(ActionType.Read)]
        public async Task<ActionResult> Details(int id, string name)
        {
            var instance = await ValidateDetailsAsync<EventInstance>(_es, id, name);

            string userRsvp = "";
            bool userAttending = false;
            string friendsAttendingSummary = "";

            try
            {
                var fb = UserFacebookClient;
                if (fb != null)
                {
                    var userId = FacebookUserId;
                    if (userId.HasValue)
                    {
                        dynamic attendingResult = await fb.GetTaskAsync(string.Format("{0}/invited?user={1}", instance.FacebookEventId, userId));
                        dynamic attending = attendingResult.data[0];
                        userAttending = attending.rsvp_status == "attending" || attending.rsvp_status == "maybe";
                        userRsvp = attending.rsvp_status;

                        var fql = string.Format("select name, url from profile where id IN (select uid FROM event_member WHERE eid = {0} and uid IN (SELECT uid2 from friend where uid1 = me()))", instance.FacebookEventId);

                        dynamic friendsAttendingResult = await fb.GetTaskAsync("fql", new { q = fql });
                        friendsAttendingSummary = FacebookUserSummaryHtmlString(friendsAttendingResult.data);
                    }
                }

            }
            catch (FacebookApiException)
            {

            }

            await SetSharedViewParameters();
            var vm = new EventInstanceViewModel()
            {
                EventInstanceId = instance.Id,
                Name = instance.Name,
                Address = instance.Venue != null ? instance.Venue.Address : "",
                DateUpdated = instance.DateUpdated,
                Description = instance.Description,
                EndDate = instance.EndDate,
                FacebookEventId = instance.FacebookEventId,
                Location = (instance.Venue != null && instance.Venue.Location != null) ? instance.Venue.Location.Name : "",
                ParentEventId = instance.Event != null ? instance.Event.Id : 0,
                Photos = instance.Photos.ToArray(),
                MainImageUrl = instance.MainImageUrl,
                StartDate = instance.StartDate,
                Url = instance.Url,
                VenueLatLng = instance.Venue != null ? instance.Venue.LatLng : null,
                VenueName = instance.Venue != null ? instance.Venue.Name : "",

                CurrentUserAttending = userAttending,
                CurrentUserRsvpStatus = userRsvp,

                FriendsAttending = friendsAttendingSummary

            };

            return View(vm);
        }

        protected string FacebookUserSummaryHtmlString(dynamic friends)
        {
            var count = friends.Count;
            if (count == 0)
            {
                return "";
            }
            if (count == 1)
                return friends[0].name;
            if (count == 2)
                return friends[0].name + " and " + friends[1].name;
            return friends[0].name + ", " + friends[1].name + " and " + count + " other friends";

        }

        [Authorize(Roles = "Admin")]
        [Route("create/")]
        [ActionType(ActionType.Create)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create/")]
        [Authorize(Roles = "Admin")]
        [ActionType(ActionType.Create)]
        public async Task<ActionResult> Create([Bind(Include = "Description,EndDate,FacebookEventId,Name,StartDate")] EventInstanceViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var instance = new EventInstance();

                instance.Description = vm.Description;
                instance.EndDate = vm.EndDate;

                if (!string.IsNullOrWhiteSpace(vm.FacebookEventId))
                {
                    try
                    {
                        var fb = AppFacebookClient;
                        dynamic eventDetails = await fb.GetTaskAsync(vm.FacebookEventId);

                        instance.FacebookEventId = vm.FacebookEventId;
                    }
                    catch (FacebookApiException)
                    {
                        instance.FacebookEventId = null;
                    }
                }

                instance.Name = vm.Name;
                instance.StartDate = vm.StartDate;
                _es.Add(instance);
                await _es.SaveAsync();
                return RedirectToAction("Details", new { id = instance.Id, name = SafeUri(instance.Name) });
            }
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [Route("edit/{id:int}")]
        [ActionType(ActionType.Update)]
        public async Task<ActionResult> Edit(int id)
        {
            var instance = await _es.GetAsync<EventInstance>(id);
            if (instance == null)
                throw new HttpException(404, "Not Found");

            var vm = new EventInstanceViewModel()
            {
                Description = instance.Description,
                EndDate = instance.EndDate,
                FacebookEventId = instance.FacebookEventId,
                Name = instance.Name,
                StartDate = instance.StartDate,
                VenueLatLng = instance.Venue != null ? instance.Venue.LatLng : null,
                VenueName = instance.Venue != null ? instance.Venue.Name : null,
                EventInstanceId = instance.Id
            };
            return View(vm);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("edit/{id:int}")]
        [ActionType(ActionType.Update)]
        public async Task<ActionResult> Edit(int id, EventInstanceViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var instance = await _es.GetAsync<EventInstance>(id);
                if (instance == null)
                    throw new HttpException(404, "Not Found");

                instance.Description = vm.Description;
                instance.EndDate = vm.EndDate;
                if (!string.IsNullOrWhiteSpace(vm.FacebookEventId))
                {
                    try
                    {
                        var fb = AppFacebookClient;
                        dynamic eventDetails = await fb.GetTaskAsync(vm.FacebookEventId);

                        instance.FacebookEventId = vm.FacebookEventId;
                    }
                    catch (FacebookApiException)
                    {
                        instance.FacebookEventId = null;
                    }
                }
                instance.Name = vm.Name;
                instance.StartDate = vm.StartDate;
                await _es.SaveAsync();
                //instance.Venue.
            }
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [Route("updatefromfacebook/{id:int}")]
        [ActionType(Core.Services.ActionType.Update)]
        public async Task<ActionResult> UpdateFromFacebook(int id)
        {
            var instance = await _es.GetAsync<EventInstance>(id);
            if (instance == null)
                throw new HttpException(404, "Not Found");
            var fb = AppFacebookClient;
            var vm = new EventInstanceUpdateStatusViewModel()
            {
                LastUpdated_Database = instance.DateUpdated,
                Event_Database = new EventInstanceViewModel()
                {
                    EventInstanceId = instance.Id,
                    Name = instance.Name,
                    Address = instance.Venue != null ? instance.Venue.Address : "",
                    DateUpdated = instance.DateUpdated,
                    Description = instance.Description,
                    EndDate = instance.EndDate,
                    FacebookEventId = instance.FacebookEventId,
                    Location = (instance.Venue != null && instance.Venue.Location != null) ? instance.Venue.Location.Name : "",
                    ParentEventId = instance.Event != null ? instance.Event.Id : 0,
                    Photos = instance.Photos.ToArray(),
                    MainImageUrl = instance.MainImageUrl,
                    StartDate = instance.StartDate,
                    Url = instance.Url,
                    VenueLatLng = instance.Venue != null ? instance.Venue.LatLng : null,
                    VenueName = instance.Venue != null ? instance.Venue.Name : ""
                }
            };

            if (fb != null)
            {
                dynamic eventDetails = await fb.GetTaskAsync(instance.FacebookEventId);

                if (eventDetails.id == instance.FacebookEventId)
                {
                    var fbVm = new EventInstanceViewModel();

                    var dateUpdated = DateTimeOffset.Parse(eventDetails.updated_time);
                    vm.LastUpdated_Facebook = dateUpdated;
                    fbVm.Description = eventDetails.description;
                    fbVm.EndDate = DateTime.Parse(eventDetails.end_time);
                    fbVm.StartDate = DateTime.Parse(eventDetails.start_time);
                    fbVm.Name = eventDetails.name;
                    fbVm.DateUpdated = dateUpdated;
                    dynamic picture = await fb.GetTaskAsync(instance.FacebookEventId + "/picture?type=large&redirect=false");

                    fbVm.MainImageUrl = picture.data.url;
                    fbVm.Url = "https://www.facebook.com/events/" + instance.FacebookEventId;
                    if (eventDetails.venue != null)
                    {
                        var venue = new Venue()
                        {
                            IsDeleted = false,
                            FacebookId = eventDetails.venue.id,
                            Location = _locationService.GetByCityCountry(eventDetails.venue.city, eventDetails.venue.country),
                            Name = eventDetails.location,
                            LatLng = new LatLng(eventDetails.venue.latitude, eventDetails.venue.longitude),
                            Address = eventDetails.venue.street
                        };
                        var fbvenue = await _venueService.GetOrCreateMatchingVenueAsync(venue);
                        if (fbvenue == null)
                            fbvenue = venue;
                        
                        fbVm.Address = fbvenue.Address;
                        if (fbvenue.Location != null)
                            fbVm.Location = instance.Venue.Location.Name;
                        fbVm.VenueName = fbvenue.Name;
                        fbVm.VenueLatLng = fbvenue.LatLng;
                        
                    }
                    vm.Event_Facebook = fbVm;
                }
            }

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("updatefromfacebook/{id:int}")]
        [ActionType(Core.Services.ActionType.Update)]
        public async Task<ActionResult> UpdateFromFacebook(int id, EventInstanceUpdateStatusViewModel vm)
        {
            var instance = await _es.GetAsync<EventInstance>(id);
            if (instance == null)
                throw new HttpException(404, "Not Found");
            /*
             * if (location == null)
            {
                location = new Location()
                {
                    Name = city,
                    Country = _es.GetAll<Country>().Single(c => c.Name == country || c.ShortName == country),
                    LatLng = new LatLng()
                };
                _es.Add(location);
            }
             * 
             
            _es.Add(venue);
            await _es.SaveAsync();
             */
            var fb = AppFacebookClient;
            if (fb != null)
            {
                dynamic eventDetails = await fb.GetTaskAsync(instance.FacebookEventId);

                if (eventDetails.id == instance.FacebookEventId)
                {
                    var dateUpdated = DateTimeOffset.Parse(eventDetails.updated_time);
                    if (vm.UpdateDescription)
                        instance.Description = eventDetails.description;
                    if (vm.UpdateEnd)
                        instance.EndDate = DateTime.Parse(eventDetails.end_time);
                    if (vm.UpdateStart)
                        instance.StartDate = DateTime.Parse(eventDetails.start_time);
                    if (vm.UpdateName)
                        instance.Name = eventDetails.name;

                    instance.DateUpdated = dateUpdated;
                    if (vm.UpdateUrl)
                        instance.Url = "https://www.facebook.com/events/" + instance.FacebookEventId;

                    if (vm.UpdateCoverImage)
                    {
                        dynamic picture = await fb.GetTaskAsync(instance.FacebookEventId + "/picture?type=large&redirect=false");
                        instance.MainImageUrl = picture.data.url;
                    }
                    if (vm.UpdateVenue)
                    {
                        if (eventDetails.venue != null)
                        {
                            var venue = new Venue()
                            {
                                IsDeleted = false,
                                FacebookId = eventDetails.venue.id,
                                Location = _locationService.GetByCityCountry(eventDetails.venue.city, eventDetails.venue.country),
                                Name = eventDetails.location,
                                LatLng = new LatLng(eventDetails.venue.latitude, eventDetails.venue.longitude),
                                Address = eventDetails.venue.street
                            };
                            var matchingVenue = await _venueService.GetOrCreateMatchingVenueAsync(venue);
                            if (matchingVenue == null)
                            {
                                _es.Add(venue);
                                matchingVenue = venue;
                            }
                            instance.Venue = matchingVenue;
                        }
                    }
                    await _es.SaveAsync();
                }
            }
            return RedirectToAction("Edit", new { id = id });
        }

    }
}