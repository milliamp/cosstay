using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class TravelService : ITravelService
    {
        public TravelInfo CalculateTravelInfo(LatLng from, LatLng to)
        {
            var info = new TravelInfo()
            {
                From = from,
                To = to,
                TravelCosts = new List<TravelCost>()
            };

            info.TravelCosts.Add(new TravelCost() {
                Method = TravelMethod.Direct,
                Distance = (decimal)GeoCodeCalc.CalcDistance(from.Lat.Value, from.Lng.Value, to.Lat.Value, to.Lng.Value, GeoCodeCalcMeasurement.Kilometers)
            });

            return info;
        }
    }
}
