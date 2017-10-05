using Bikewale.Entities.BikeData;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : BAL interface for bike series
    /// </summary>
    public interface IBikeSeriesRepository
    {
        BikeSeriesModels GetModelsListBySeriesId(uint seriesId);
    }
}
