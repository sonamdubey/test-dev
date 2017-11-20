using Bikewale.Entities.BikeData;

namespace Bikewale.Models.BikeSeries
{
    public class SeriesPageVM : ModelBase
    {
        public BikeSeriesModels SeriesModels { get; set; }

        public BikeSeriesCompareVM ObjModel { get; set; }

        public UsedBikeByModelCityVM objUsedBikes { get; set; }

    }
}
