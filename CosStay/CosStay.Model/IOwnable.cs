﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{
    public interface IOwnable
    {
        string OwnerId { get; }
    }
}
