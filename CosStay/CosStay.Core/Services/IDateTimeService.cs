using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services
{
    public interface IDateTimeService
    {
        DateTimeOffset Now { get; }
        DateTime Today { get; }
    }
}
