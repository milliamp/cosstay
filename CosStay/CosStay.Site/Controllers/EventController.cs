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

namespace CosStay.Site.Controllers
{
    public class EventController : BaseController
    {
        //
        // GET: /Event/
        public ActionResult Index()
        {

            using (var db = new CosStayContext())
            {
                if (db.Events.Count() > 0)
                {
                    db.Events.RemoveRange(db.Events);
                    db.EventInstances.RemoveRange(db.EventInstances);
                    
                }
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

        [Authorize]
        public ActionResult Details(string id)
        {
            int eventId = 0;
            if (!int.TryParse(id, out eventId))
                throw new InvalidOperationException();

            using (var db = new CosStayContext())
            {
                var instance = db.EventInstances.Find(eventId);
                if (instance == null)
                    throw new FileNotFoundException();

                string userRsvp = "";
                bool userAttending = false;

                var fb = FacebookClient;
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
                            db.SaveChanges();
                        }
                    }
                    var userId = FacebookUserId;
                    if (userId.HasValue)
                    {
                        dynamic attendingResult = fb.Get(string.Format("{0}/invited?user={1}", instance.FacebookEventId, userId));
                        dynamic attending = attendingResult.data[0];
                        userAttending = attending.rsvp_status == "attending" || attending.rsvp_status == "maybe";
                        userRsvp = attending.rsvp_status;
                    }
                }
                    
                var vm = new EventInstanceViewModel
                {
                    EventInstanceId = instance.EventInstanceId,
                    Name = instance.Name,
                    Address = instance.Venue != null ? instance.Venue.Address : "",
                    DateUpdated = instance.DateUpdated,
                    Description = instance.Description,
                    EndDate = instance.EndDate,
                    FacebookEventId = instance.FacebookEventId,
                    Location = instance.Venue != null ? instance.Venue.Location.Name : "",
                    ParentEventId = instance.Event.EventId,
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
            
        }
	}
}