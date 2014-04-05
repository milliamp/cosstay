using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class AppFacebookService : FacebookServiceBase, IAppFacebookService
    {

        public AppFacebookService(IDateTimeService dateTimeService, string appId, string appSecret)
            :base (dateTimeService, appId, appSecret)
        {
        }


        protected override FacebookCredentials GetClientCredentials()
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("oauth/access_token", new
            {
                client_id = AppId,
                client_secret = AppSecret,
                grant_type = "client_credentials"
            });

            return new FacebookCredentials
            {
                AccessToken = result.access_token
            };
        }
    }
}
