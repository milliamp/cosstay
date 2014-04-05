using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class DateTimeService: IDateTimeService
    {
        public DateTimeOffset Now
        {
            get { return DateTimeOffset.Now; }
        }

        public DateTime Today
        {
            get { return DateTime.Today; }
        }
    }
}
