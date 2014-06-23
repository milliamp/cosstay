using CosStay.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class EventInstanceViewModel
    {
        public int Id { get; set; }
        public int ParentEventId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Website")]
        public string Url { get; set; }
        public string FacebookEventId { get; set; }
        public string Description { get; set; }

        [Display(Name = "Venue")]
        public string VenueName { get; set; }

        [Display(Name = "Location")]
        public LatLng VenueLatLng { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }

        [Display(Name = "Starts")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ends")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTimeOffset DateUpdated { get; set; }

        public string MainImageUrl { get; set; }
        public IEnumerable<Photo> Photos { get; set; }

        public string CurrentUserRsvpStatus { get; set; }
        public bool CurrentUserAttending { get; set; }

        public string FriendsAttending { get; set; }
    }
}