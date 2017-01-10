using Bikewale.Entities.GenericBikes;

namespace Bikewale.Interfaces.GenericBikes
{
    /// <summary>
    /// Created By : Sushil Kumar on 2nd Jan 2016
    /// Description : Interface for generic bike info 
    /// </summary>
    public interface IGenericBikeRepository
    {
        GenericBikeInfo GetGenericBikeInfo(uint modelId);
    }
}
