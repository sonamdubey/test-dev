using Bikewale.Entities.UsedBikes;
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
    }
}
