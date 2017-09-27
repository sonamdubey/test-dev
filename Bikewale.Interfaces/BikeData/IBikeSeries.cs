using Bikewale.Entities.BikeData;

namespace Bikewale.Interfaces.BikeData
{
    public interface IBikeSeries
    {
        BikeSeriesModels GetModelsListBySeriesId(int modelId, uint seriesId);
    }
}
