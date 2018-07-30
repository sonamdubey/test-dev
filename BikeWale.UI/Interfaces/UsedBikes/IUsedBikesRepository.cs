using Bikewale.Entities.BikeData;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UsedBikes
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 21 june 2016
    /// Desc : reference added for used bike repository
    /// modified by Subodh Jain 14 sep 2106
    /// des:-added function to fetch most recent used bikes by diff condtions
    /// </summary>
    public interface IUsedBikesRepository
    {
        IEnumerable<PopularUsedBikesEntity> GetPopularUsedBikes(uint totalCount, int? city = null);
        IEnumerable<MostRecentBikes> GetUsedBikesbyMake(uint makeId, uint totalCount);
        IEnumerable<MostRecentBikes> GetUsedBikesbyModel(uint modelId, uint totalCount);
        IEnumerable<MostRecentBikes> GetUsedBikesbyModelCity(uint modelId, uint cityId, uint totalCount);
        IEnumerable<MostRecentBikes> GetUsedBikesbyMakeCity(uint makeId, uint cityId, uint totalCount);
        IEnumerable<UsedBikeMakeEntity> GetUsedBikeMakesWithCount();
        IEnumerable<MostRecentBikes> GetUsedBikesSeries(uint seriesId, uint cityId);
    }
}
