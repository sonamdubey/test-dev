
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;
namespace Bikewale.Interfaces.UsedBikes
{
    /// <summary>
    /// Author : Vivek Gupta
    /// Date : 21st june 2016
    /// Desc : most recent used bikes
    /// </summary>
    public interface IUsedBikesCacheRepository
    {
        IEnumerable<MostRecentBikes> GetMostRecentUsedBikes(uint makeId, uint topCount, int? cityId);
    }
}
