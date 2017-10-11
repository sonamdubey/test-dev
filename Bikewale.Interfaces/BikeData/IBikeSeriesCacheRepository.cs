using Bikewale.Entities.BikeData;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 28th Sep 2017
    /// Summary : Cache Repo interface for bike series
    /// </summary>
    public interface IBikeSeriesCacheRepository
    {
        BikeSeriesModels GetModelsListBySeriesId(uint seriesId);
    }
}
