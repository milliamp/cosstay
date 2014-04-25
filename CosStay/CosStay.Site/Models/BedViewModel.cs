using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class BedViewModel {
        /*public BedViewModel(Bed bed)
        {
            BedId = bed.BedId;
            BedSizeId = bed.Size.BedSizeId;
            BedTypeId = bed.Type.BedTypeId;
            DateAdded = bed.DateAdded;
            Photos = bed.Photos;
        }*/

        public int Id { get; set; }
        public int BedSizeId { get; set; }
        public int BedTypeId { get; set; }
        public DateTimeOffset DateAdded { get; set; }
        public List<Photo> Photos { get; set; }
    }
}