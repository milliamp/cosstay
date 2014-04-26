using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LatLng LatLng { get; set; }

    }
}