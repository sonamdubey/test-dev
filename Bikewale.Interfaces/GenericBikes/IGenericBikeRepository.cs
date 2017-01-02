using Bikewale.Entities.GenericBikes;

namespace Bikewale.Interfaces.GenericBikes
{
    public interface IGenericBikeRepository
    {
        GenericBikeInfo GetGenericBikeInfo(uint modelId);
    }
}
