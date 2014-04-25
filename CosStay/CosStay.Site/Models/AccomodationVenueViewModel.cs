using CosStay.Model;
using System;
using System.Collections.Generic;
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
        public Location Location { get; set; }

        public UserViewModel Owner { get; set; }
        public DateTimeOffset DateAdded { get; set; }

        public List<AccomodationVenueFeature> Features { get; set; }
        public bool AllowsBedSharing { get; set; }
        public bool AllowsMixedRooms { get; set; }

        public List<Photo> Photos { get; set; }
        public List<AccomodationRoomViewModel> Rooms { get; set; }


        public List<BedSize> AvailableBedSizes { get; set; }
        public List<BedType> AvailableBedTypes { get; set; }
    }
}