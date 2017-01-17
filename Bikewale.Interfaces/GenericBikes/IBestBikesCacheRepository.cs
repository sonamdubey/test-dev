using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.Interfaces.GenericBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 26 Dec 2016
    /// Description :   Interface for Best Bikes Cache Repository
    /// Modified By : Sushil Kumar on 2nd Jan 2016
    /// Description : Addded new interface input parameter for generic bike info
    /// Modified By : Sushil Kumar on 12 Jan 2017
    /// Description : Addded new method for get bike ranking by model id
    /// </summary>
    public interface IBestBikesCacheRepository
    {
        SearchOutputEntity BestBikesByType(EnumBikeBodyStyles bodyStyle, FilterInput filterInputs, InputBaseEntity input);
        GenericBikeInfo GetGenericBikeInfo(uint modelId);
        BikeRankingEntity GetBikeRankingByCategory(uint modelId);
    }
}
