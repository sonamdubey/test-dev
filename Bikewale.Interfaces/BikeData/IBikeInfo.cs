using Bikewale.Entities.GenericBikes;
using Bikewale.Models.Shared;

namespace Bikewale.Interfaces.BikeData
{
    /// Created  By :- subodh Jain 10 Feb 2017
    /// Summary :- BikeInfo Slug details
    /// </summary>
    public interface IBikeInfo
    {
        BikeInfo GetBikeInfo(uint modelId);
        GenericBikeInfo GetBikeInfo(uint modelId, uint cityId);
    }
}
