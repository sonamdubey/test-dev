using Bikewale.Entities.GenericBikes;
using Bikewale.Models.Shared;

namespace Bikewale.Interfaces.BikeData
{
    /// Created  By :- subodh Jain 10 Feb 2017
    /// Summary :- BikeInfo Slug details
    /// Modified By : Sanjay George on 1 Oct 2018
    /// Description : Added method GetBikeInfo with different params
    /// </summary>
    public interface IBikeInfo
    {
        BikeInfo GetBikeInfo(uint modelId);
        GenericBikeInfo GetBikeInfo(uint modelId, uint cityId, bool isUsedBikeFetched);
    }
}
