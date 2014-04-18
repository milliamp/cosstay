using CosStay.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class UserService : IUserService
    {
        protected UserManager<User> _manager;
        public UserService(IEntityStore entityStore)
        {
            _manager = new UserManager<User>(new UserStore(entityStore));
        }
        public Microsoft.AspNet.Identity.UserManager<Model.User> UserManager
        {
            get { return _manager; }
        }
    }
}
