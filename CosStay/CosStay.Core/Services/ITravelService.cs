using CosStay.Model;
using System;
namespace CosStay.Core.Services
{
    public interface ITravelService
    {
        TravelInfo CalculateTravelInfo(LatLng from, LatLng to);
    }
}
