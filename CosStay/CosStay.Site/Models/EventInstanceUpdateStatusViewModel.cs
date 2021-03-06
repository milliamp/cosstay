﻿using System;
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
        public bool UpdateName { get; set; }
        public bool UpdateUrl { get; set; }
        public bool UpdateDescription { get; set; }
        public bool UpdateCoverImage { get; set; }
        public bool UpdateVenue { get; set; }
        public bool UpdateStart { get; set; }
        public bool UpdateEnd { get; set; }

        public bool ChangesToName { get; set; }
        public bool ChangesToUrl { get; set; }
        public bool ChangesToDescription { get; set; }
        public bool ChangesToCoverImage { get; set; }
        public bool ChangesToVenue { get; set; }
        public bool ChangesToStart { get; set; }
        public bool ChangesToEnd { get; set; }

    }
}