using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class UserFacebookService:FacebookServiceBase, IUserFacebookService
    {
        private ClaimsIdentity Identity { get; set; }
        public UserFacebookService(IDateTimeService dateTimeService, ClaimsIdentity identity, string appId, string appSecret)
            :base (dateTimeService, appId, appSecret)
        {
            Identity = identity;
        }

        protected override FacebookCredentials GetClientCredentials()
        {
            var c = Identity.FindFirst("FacebookAccessToken");
            if (c != null)
                return new FacebookCredentials
                {
                    AccessToken = c.Value
                };
            
            return null;
        }
    }
}
