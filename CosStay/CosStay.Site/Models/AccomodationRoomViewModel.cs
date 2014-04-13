using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CosStay.Site.Models
{
    public class AccomodationRoomViewModel
    {
        public AccomodationRoomViewModel()
        {

        }
        /*public AccomodationRoomViewModel(AccomodationRoom room)
        {
            AccomodationRoomId = room.AccomodationRoomId;
            Beds = room.Beds.Select(b => new BedViewModel(b)).ToList();
            Features = room.Features;
            DateAdded = room.DateAdded;
            Photos = room.Photos;
        }*/


        public int Id { get; set; }
        public string Name { get; set; }
        public AccomodationVenue AccomodationVenue { get; set; }
        public List<BedViewModel> Beds { get; set; }

        public List<AccomodationRoomFeature> Features { get; set; }

        public DateTimeOffset DateAdded { get; set; }

        public virtual List<Photo> Photos { get; set; }

        public bool IsDeleted { get; set; }

    }
}
