using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{
    public class SeedData
    {
        public int SeedDataId { get; set; }
        public string Category { get; set; }
        public DateTimeOffset Version { get; set; }
    }
}
