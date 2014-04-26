using CosStay.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class AccomodationVenueViewModel {
        /*
        public AccomodationVenueViewModel(AccomodationVenue venue)
        {
            AccomodationVenueId = venue.AccomodationVenueId;
            LatLng = venue.LatLng;
            Address = venue.Address;
            Location = venue.Location;
            Owner = new UserViewModel(venue.Owner);
            DateAdded = venue.DateAdded;
            Features = venue.Features;
            AllowsBedSharing = venue.AllowsBedSharing;
            AllowsMixedRooms = venue.AllowsMixedRooms;
            Photos = venue.Photos;
            Rooms = venue.Rooms.Select(r => new AccomodationRoomViewModel(r)).ToList();
        }*/

        public int Id { get; set; }
        public string Name { get; set; }
        public LatLng LatLng { get; set; }
        public string Address { get; set; }
        [Display(Name = "Address to show on site")]
        public string PublicAddress { get; set; }
        public LocationViewModel Location { get; set; }

        public UserViewModel Owner { get; set; }
        [Display(Name = "Date Added")]
        public DateTimeOffset DateAdded { get; set; }

        public List<AccomodationVenueFeature> Features { get; set; }
        [Display(Name = "Allows Bed Sharing")]
        public bool AllowsBedSharing { get; set; }
        [Display(Name = "Allows Mixed Rooms")]
        public bool AllowsMixedRooms { get; set; }

        public List<Photo> Photos { get; set; }
        public List<AccomodationRoomViewModel> Rooms { get; set; }
        
        [Display(Name = "Number of Beds")]
        public int TotalBeds { get; set; }

        public Dictionary<EventInstance, TravelInfo> TravelInfo { get; set; }

        public List<BedSize> AvailableBedSizes { get; set; }
        public List<BedType> AvailableBedTypes { get; set; }
    }
}