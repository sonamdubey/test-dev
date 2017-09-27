using Bikewale.Entities.BikeData;

namespace Bikewale.Interfaces.BikeData
{
    public interface IBikeSeriesRepository
    {
        BikeSeriesModels GetModelsListBySeriesId(uint seriesId);
    }
}
