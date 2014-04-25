using CosStay.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class UserService : IUserService
    {
        protected UserManager<User> _manager;
        protected User _user;
        public UserService(IEntityStore entityStore, IPrincipal principal)
        {
            _manager = new UserManager<User>(new UserStore(entityStore));
            _user = AsyncInline.Run<User>(async () => await entityStore.GetAsync<User>(principal.Identity.GetUserId()));
        }
        public Microsoft.AspNet.Identity.UserManager<Model.User> UserManager
        {
            get { return _manager; }
        }

        public User CurrentUser
        {
            get
            {
                return _user;
            }
        }
    }
}
