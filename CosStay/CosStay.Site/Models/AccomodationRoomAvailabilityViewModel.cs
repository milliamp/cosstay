using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class AccomodationRoomAvailabilityViewModel
    {
        public int Id { get; set; }
        public List<DateTime> Nights { get; set; }
        public List<BedAvailabilityViewModel> BedAvailability { get; set; }
        public List<BedViewModel> Beds { get; set; }
    }
}