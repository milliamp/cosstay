using CosStay.Model;
using System;
using System.Threading.Tasks;
namespace CosStay.Core.Services
{
    public interface IVenueService
    {
        Task<Venue> GetOrCreateMatchingVenueAsync(CosStay.Model.Venue venue);
    }
}
