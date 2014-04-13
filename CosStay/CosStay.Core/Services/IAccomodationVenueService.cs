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
        Model.AccomodationVenue UpdateVenue(AccomodationVenue venue);
    }
}
