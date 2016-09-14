﻿using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UsedBikes
{
    /// <summary>
    /// Created By Sajal Gupta On 14/09/2016
    /// Description : Function to getBikes based on model/city/make.
    /// </summary>
    public interface IUsedBikes
    {
        IEnumerable<MostRecentBikes> GetPopularUsedBikes(uint makeId, uint modelId, uint cityId, uint totalCount);

        IEnumerable<MostRecentBikes> GetUsedBikesbyMake(uint makeId, uint totalCount);
        IEnumerable<MostRecentBikes> GetUsedBikesbyModel(uint modelId, uint totalCount);
        IEnumerable<MostRecentBikes> GetUsedBikesbyModelCity(uint modelId, uint cityId, uint totalCount);
        IEnumerable<MostRecentBikes> GetUsedBikesbyMakeCity(uint makeId, uint cityId, uint totalCount);
    }
}
