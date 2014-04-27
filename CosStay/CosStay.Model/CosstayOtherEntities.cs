using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{
    public class TravelInfo
    {
        public virtual LatLng From { get; set; }
        public virtual LatLng To { get; set; }
        public virtual List<TravelCost> TravelCosts { get; set; }
    }
    public class TravelCost
    {
        public virtual TravelMethod Method { get; set; }
        public decimal? Distance { get; set; }
        public decimal? Price { get; set; }
    }
    public enum TravelMethod
    {
        Direct,
        Walking,
        Cycling,
        Driving,
        PublicTransport,
        Taxi
    };

    public class VenueAvailabilitySearchResult
    {
        public virtual AccomodationVenue Venue { get; set; }
        public virtual IEnumerable<AccomodationBedAvailabilityNight> Nights { get; set; }
    }
}
