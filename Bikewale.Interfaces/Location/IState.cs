﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Location;

namespace Bikewale.Interfaces.Location
{
    /// <summary>
    /// Created By : Ashish G. Kamble
    /// </summary>
    public interface IState
    {
        List<StateEntityBase> GetStates();
        Hashtable GetMaskingNames();
    }
}
