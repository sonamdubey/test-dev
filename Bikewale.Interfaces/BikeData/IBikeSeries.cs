using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : BAL interface for bike series
    /// Modified by : Ashutosh Sharma on 17 Nov 2017
    /// Description : Added GetNewModels, GetUpcomingModels, GetOtherSeriesFromMake, GetSynopsis.
    /// Modified by : vivek singh tomar on 24th nov 2017
    /// Summary : added GetModelIdsBySeries
    /// Modified By : Deepak Israni on 16 April 2018
    /// Description : Added GetMakeSeries
    /// </summary>
    public interface IBikeSeries
    {
        BikeSeriesModels GetModelsListBySeriesId(uint modelId, uint seriesId);
		IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId);
		IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId);
		BikeDescriptionEntity GetSynopsis(uint seriesId);
		IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId, uint seriesId);
		IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId);
        string GetModelIdsBySeries(uint seriesId);
        IEnumerable<BikeSeriesEntity> GetMakeSeries(uint makeId, uint cityId);

    }
}
