using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UsedBikes
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 21 june 2016
    /// Desc : reference added for used bike repository
    /// </summary>
    public interface IUsedBikes
    {
        IEnumerable<PopularUsedBikesEntity> GetPopularUsedBikes(uint totalCount, int? city = null);
        IEnumerable<MostRecentBikes> GetMostRecentUsedBikes(uint makeId, uint totalCount, int? cityId = null);
    }
}
