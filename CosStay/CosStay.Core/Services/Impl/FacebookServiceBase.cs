using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public abstract class FacebookServiceBase
    {
        private IDateTimeService _dateTimeService { get; set; }
        protected string AppId { get; set; }
        protected string AppSecret { get; set; }

        public FacebookServiceBase(IDateTimeService dateTimeService, string appId, string appSecret)
        {
            _dateTimeService = dateTimeService;
            AppId = appId;
            AppSecret = appSecret;
        }

        protected string AccessToken { get; set; }
        protected DateTimeOffset Expiry { get; set; }
        protected FacebookClient _client;

        public FacebookClient Client
        {
            get
            {
                var expired = Expiry != null ? Expiry < _dateTimeService.Now : false;
                if (_client == null || expired)
                {
                    if (AccessToken == null || expired)
                    {
                        if (!GetNewAccessToken())
                            return null;
                    }
                    _client = new FacebookClient(AccessToken)
                    {
                        AppId = AppId,
                        AppSecret = AppSecret
                    };
                }
                return _client;
            }
        }

        private bool GetNewAccessToken()
        {
            var clientCredentials = GetClientCredentials();
            if (clientCredentials == null)
                return false;
            AccessToken = clientCredentials.AccessToken;
            Expiry = clientCredentials.Expiry;
            return true;
        }

        protected abstract FacebookCredentials GetClientCredentials();
    }
}
