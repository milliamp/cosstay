using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class BedAvailabilityViewModel : BedViewModel
    {
        public List<Tuple<DateTime, BedStatus>> Nights;
    }
}