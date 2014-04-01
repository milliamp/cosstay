using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class EventInstanceViewModel
    {
        public int EventInstanceId { get; set; }
        public int ParentEventId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string FacebookEventId { get; set; }
        public string Description { get; set; }

        public string VenueName { get; set; }
        public LatLng VenueLatLng { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTimeOffset DateUpdated { get; set; }

        public IEnumerable<Photo> Photos { get; set; }

        public string CurrentUserRsvpStatus { get; set; }
        public bool CurrentUserAttending { get; set; }
    }
}