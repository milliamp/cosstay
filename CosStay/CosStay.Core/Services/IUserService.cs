﻿using CosStay.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services
{
    public interface IUserService
    {
        UserManager<User> UserManager { get; }
        User CurrentUser { get; }
    }
}
