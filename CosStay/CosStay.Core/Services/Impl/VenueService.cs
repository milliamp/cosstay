using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class VenueService : IVenueService
    {
        private IEntityStore _es;
        public VenueService(IEntityStore entityStore)
        {
            _es = entityStore;
        }

        public async Task<Venue> GetOrCreateMatchingVenueAsync(Venue venue)
        {
            Venue matchingVenue = null;
            if (!string.IsNullOrWhiteSpace(venue.FacebookId))
            {
                // TODO: Async
                matchingVenue = _es.GetAll<Venue>().SingleOrDefault(v => v.FacebookId == venue.FacebookId);
                if (matchingVenue != null)
                    return matchingVenue;
            }

            if (venue.LatLng.Lat.HasValue && venue.LatLng.Lng.HasValue)
            {
                // TODO: Make fuzzy
                // TODO: Async
                matchingVenue = _es.GetAll<Venue>().SingleOrDefault(v => v.LatLng.Lat == venue.LatLng.Lat && v.LatLng.Lng == venue.LatLng.Lng);
                if (matchingVenue != null)
                    return matchingVenue;
            }
            if (!string.IsNullOrWhiteSpace(venue.Name))
            {
                // TODO: Async
                matchingVenue = _es.GetAll<Venue>().SingleOrDefault(v => v.Name == venue.Name);
                if (matchingVenue != null)
                    return matchingVenue;
            }

            return matchingVenue;
        }
    }
}
