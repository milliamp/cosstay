using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services
{
    public interface IAccomodationVenueService
    {
        Task<Model.AccomodationVenue> UpdateVenueAsync(AccomodationVenue venue);
        Task<IQueryable<VenueAvailabilitySearchResult>> AvailableVenuesQueryAsync(int eventInstanceId);
    }
}
