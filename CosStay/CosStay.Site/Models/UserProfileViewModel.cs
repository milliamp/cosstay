using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site.Models
{
    public class UserProfileViewModel
    {
        public UserViewModel User { get; set; }
        public string UserInterests { get; set; }
    }
}