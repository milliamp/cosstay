using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class PhotoViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
    }
}