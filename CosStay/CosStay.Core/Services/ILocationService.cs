using System;
namespace CosStay.Core.Services
{
    public interface ILocationService
    {
        CosStay.Model.Location GetByCityCountry(string city, string country);
    }
}
