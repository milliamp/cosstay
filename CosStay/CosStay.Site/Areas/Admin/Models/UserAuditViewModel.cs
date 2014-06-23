using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site.Areas.Admin.Models
{
    public class UserAuditViewModel
    {
        public User User { get; set; }
        public Audit[] Audits { get; set; }
    }
}