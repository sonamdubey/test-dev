using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Modified By  : Sushil Kumar on 28th June 2016
    /// Description : Added Upcoming bikes method
    /// Modified By :   Sumit Kate on 01 Jul 2016
    /// Description :   GetMostPopularBikes Method
    /// </summary>
    /// <typeparam name="U"></typeparam>
    public interface IBikeModelsCacheRepository<U>
    {
        BikeModelPageEntity GetModelPageDetails(U modelId);
        IEnumerable<UpcomingBikeEntity> GetUpcomingBikesList(EnumUpcomingBikesFilter sortBy, int pageSize, int? makeId = null, int? modelId = null, int? curPageNo = null);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikesByMake(int makeId);
        IEnumerable<MostPopularBikesBase> GetMostPopularBikes(int? topCount = null, int? makeId = null);
    }
}
