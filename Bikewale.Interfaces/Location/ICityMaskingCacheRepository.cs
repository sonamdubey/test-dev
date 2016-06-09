﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.Location;

namespace Bikewale.Interfaces.Location
{
    public interface ICityMaskingCacheRepository
    {
        CityMaskingResponse GetCityMaskingResponse(string maskingName);
    }
}
