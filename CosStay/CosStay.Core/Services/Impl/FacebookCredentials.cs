using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CosStay.Core.Services.Impl
{
    public class FacebookCredentials
    {
        public string AccessToken { get; set; }
        public DateTimeOffset Expiry { get; set; }
    }
}
