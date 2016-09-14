using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UsedBikes
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 21 june 2016
    /// Desc : reference added for used bike repository
    /// </summary>
    public interface IUsedBikesRepository
    {
        IEnumerable<PopularUsedBikesEntity> GetPopularUsedBikes(uint totalCount, int? city = null);
        IEnumerable<MostRecentBikes> GetMostRecentUsedBikes(uint makeId, uint totalCount, int? cityId = null);
        IEnumerable<MostRecentBikes> GetUsedBikesbyMake(uint makeId, uint totalCount);
        IEnumerable<MostRecentBikes> GetUsedBikesbyModel(uint modelId, uint totalCount);
        IEnumerable<MostRecentBikes> GetUsedBikesbyModelCity(uint modelId, uint cityId, uint totalCount);
        IEnumerable<MostRecentBikes> GetUsedBikesbyMakeCity(uint makeId, uint cityId, uint totalCount);
    }
}
