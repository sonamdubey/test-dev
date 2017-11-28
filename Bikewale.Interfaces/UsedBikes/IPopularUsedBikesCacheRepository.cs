using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Interfaces.UsedBikes
{
    public interface IPopularUsedBikesCacheRepository
    {
        IEnumerable<PopularUsedBikesEntity> GetPopularUsedBikes(uint topCount, int? cityId);
    }
}
