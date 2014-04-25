using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class EventInstanceUpdateStatusViewModel
    {
        public DateTimeOffset LastUpdated_Database { get; set; }
        public DateTimeOffset LastUpdated_Facebook { get; set; }
        public EventInstanceViewModel Event_Database { get; set; }
        public EventInstanceViewModel Event_Facebook { get; set; }
    }
}