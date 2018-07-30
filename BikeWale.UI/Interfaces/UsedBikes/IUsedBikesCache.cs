using Bikewale.Entities.BikeData;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UsedBikes
{
    /// <summary>
    /// Author : Sajal Gupta
    /// Date : 14/09/2016
    /// Desc : Function to getBikes based on model/city/make.
    /// </summary>
    public interface IUsedBikesCache
    {
        IEnumerable<MostRecentBikes> GetUsedBikes(uint makeId, uint modelId, uint cityId, uint totalCount);
        IEnumerable<UsedBikeMakeEntity> GetUsedBikeMakesWithCount();
        IEnumerable<MostRecentBikes> GetUsedBikesSeries(uint seriesid, uint cityId);
    }
}
