using CosStay.Model;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

namespace CosStay.Site.Controllers
{
    [RoutePrefix("events")]
    [Route("{action=index}")]
    public class EventController : BaseController
    {
        public EventController(IAppFacebookService appFacebookService, IUserFacebookService userFacebookService, IEntityStore entityStore)
            : base(entityStore)
        {
            _appFacebookService = appFacebookService;
            _userFacebookService = userFacebookService;
        }

        private IAppFacebookService _appFacebookService;
        public FacebookClient AppFacebookClient
        {
            get
            {
                return _appFacebookService.Client;
            }
        }

        public IUserFacebookService _userFacebookService { get; set; }
        public FacebookClient UserFacebookClient
        {
            get
            {
                return _userFacebookService.Client;
            }
        }

        public ActionResult Index()
        {

            using (var db = new CosStayContext())
            {
                if (db.Events.Count() == 0)
                {
                    var event1 = new Event()
                    {
                        Name = "Hamilton Armageddon"

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
                    instance1.Event = event1;
                    db.Events.Add(event1);
                    db.EventInstances.Add(instance1);
                    db.SaveChanges();
                }
                return View(db.EventInstances.ToArray());
            }

        }

        [Route("{id:int}/{name?}")]
        public ActionResult Details(int id, string name)
        {
                var instance = ValidateDetails<EventInstance>(_es, id, name);

                string userRsvp = "";
                bool userAttending = false;

                try
                {
                    var fb = AppFacebookClient;
                    if (fb != null)
                    {
                        dynamic eventDetails = fb.Get(instance.FacebookEventId);

                        if (eventDetails.id == instance.FacebookEventId)
                        {
                            var dateUpdated = DateTimeOffset.Parse(eventDetails.updated_time);
                            if (instance.DateUpdated < dateUpdated)
                            {
                                instance.Description = eventDetails.description;
                                instance.EndDate = DateTime.Parse(eventDetails.end_time);
                                instance.StartDate = DateTime.Parse(eventDetails.start_time);
                                instance.Name = eventDetails.name;
                                instance.DateUpdated = dateUpdated;
                                _es.Save();
                            }
                        }

                        fb = UserFacebookClient;
                        if (fb != null)
                        {
                            var userId = FacebookUserId;
                            if (userId.HasValue)
                            {
                                dynamic attendingResult = fb.Get(string.Format("{0}/invited?user={1}", instance.FacebookEventId, userId));
                                dynamic attending = attendingResult.data[0];
                                userAttending = attending.rsvp_status == "attending" || attending.rsvp_status == "maybe";
                                userRsvp = attending.rsvp_status;
                            }
                        }
                    }
                }
                catch (FacebookApiException)
                {

                }

                var vm = new EventInstanceViewModel
                {
                    EventInstanceId = instance.Id,
                    Name = instance.Name,
                    Address = instance.Venue != null ? instance.Venue.Address : "",
                    DateUpdated = instance.DateUpdated,
                    Description = instance.Description,
                    EndDate = instance.EndDate,
                    FacebookEventId = instance.FacebookEventId,
                    Location = instance.Venue != null ? instance.Venue.Location.Name : "",
                    ParentEventId = instance.Event != null ? instance.Event.Id : 0,
                    Photos = instance.Photos.ToArray(),
                    StartDate = instance.StartDate,
                    Url = instance.Url,
                    VenueLatLng = instance.Venue != null ? instance.Venue.LatLng : null,
                    VenueName = instance.Venue != null ? instance.Venue.Name : "",

                    CurrentUserAttending = userAttending,
                    CurrentUserRsvpStatus = userRsvp
                };

                return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [Route("create/")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create/")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include="Description,EndDate,FacebookEventId,Name,StartDate")] EventInstanceViewModel vm)
        {
                if (ModelState.IsValid)
                {
                    using (var db = new CosStayContext())
                    {
                        var instance = new EventInstance();

                        instance.Description = vm.Description;
                        instance.EndDate = vm.EndDate;

                        if (!string.IsNullOrWhiteSpace(vm.FacebookEventId))
                        {
                            try
                            {
                                var fb = AppFacebookClient;
                                dynamic eventDetails = fb.Get(vm.FacebookEventId);

                                instance.FacebookEventId = vm.FacebookEventId;
                            }
                            catch (FacebookApiException)
                            {
                                instance.FacebookEventId = null;
                            }
                        }

                        instance.Name = vm.Name;
                        instance.StartDate = vm.StartDate;
                        db.EventInstances.Add(instance);
                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = instance.Id, name = SafeUri(instance.Name)});
                    }
                }
                return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [Route("edit/{id:int}")]
        public ActionResult Edit(int id)
        {
            using (var db = new CosStayContext())
            {
                var instance = db.EventInstances.Find(id);
                if (instance == null)
                    throw new HttpException(404, "Not Found");

                var vm = new EventInstanceViewModel
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
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("edit/{id:int}")]
        public ActionResult Edit(int id, EventInstanceViewModel vm)
        {
            if (ModelState.IsValid)
            {
                using (var db = new CosStayContext())
                {
                    var instance = db.EventInstances.Find(id);
                    if (instance == null)
                        throw new HttpException(404, "Not Found");

                    instance.Description = vm.Description;
                    instance.EndDate = vm.EndDate;
                    if (!string.IsNullOrWhiteSpace(vm.FacebookEventId))
                    {
                        try
                        {
                            var fb = AppFacebookClient;
                            dynamic eventDetails = fb.Get(vm.FacebookEventId);

                            instance.FacebookEventId = vm.FacebookEventId;
                        }
                        catch (FacebookApiException)
                        {
                            instance.FacebookEventId = null;
                        }
                    }
                    instance.Name = vm.Name;
                    instance.StartDate = vm.StartDate;
                    db.SaveChanges();
                    //instance.Venue.
                }
            }
            return View(vm);
        }

    }
}