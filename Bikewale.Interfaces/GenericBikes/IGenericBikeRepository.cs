using Bikewale.Entities.GenericBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.GenericBikes
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Description : Interface for generic bike info 
    /// Modified by : Aditi Srivastava on 12 Jan 2017
    /// Description : Added methods to get bike ranking by model and list of best bikes by category
    /// </summary>
    public interface IGenericBikeRepository
    {
        GenericBikeInfo GetGenericBikeInfo(uint modelId);
        BikeRankingEntity GetBikeRankingByCategory(uint modelId);
         //  IEnumerable<BestBikeEntityBase> GetBestBikeByCategory(EnumBikeBodyStyles categoryId);
    }
}
