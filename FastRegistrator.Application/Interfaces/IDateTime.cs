﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }

}
