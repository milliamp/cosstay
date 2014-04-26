using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class LocationService : ILocationService
    {
        private IEntityStore _es;
        public LocationService(IEntityStore entityStore)
        {
            _es = entityStore;
        }

        // TODO: async
        public Location GetByCityCountry(string city, string country)
        {
            var location = _es.GetAll<Location>().SingleOrDefault(l => l.Country.Name == country && l.Name == city);
            
            return location;
        }

        public LatLng Approximate(LatLng latlng)
        {
            double fuzz = 100;
            return new LatLng(Math.Round(latlng.Lat.Value * fuzz) / fuzz, Math.Round(latlng.Lng.Value * fuzz) / fuzz);
        }
    }
}
