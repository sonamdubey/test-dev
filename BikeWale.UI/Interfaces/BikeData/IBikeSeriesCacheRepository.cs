using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 28th Sep 2017
    /// Summary : Cache Repo interface for bike series
    /// Modified by : Ashutosh Sharma on 17 Nov 2017
    /// Description : Added GetNewModels, GetUpcomingModels, GetOtherSeriesFromMake, GetSynopsis.
    /// Modified by : vivek singh tomar on 24th nov 2017
    /// summary : added GetModelIdsBySeries
    /// Modified By : Deepak Israni on 16 April 2018
    /// Description : Added GetMakeSeries
    /// </summary>
    public interface IBikeSeriesCacheRepository
    {
        BikeSeriesModels GetModelsListBySeriesId(uint seriesId, uint cityId = 0);
		IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId);
        IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId);
		IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId);
		BikeDescriptionEntity GetSynopsis(uint seriesId);
		IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId);
        string GetModelIdsBySeries(uint seriesId);
        IEnumerable<BikeSeriesEntity> GetMakeSeries(int makeId, int cityId);
        /// <summary>
        /// Written By : Ashish G. Kamble on 20 Nov 2017
        /// Summary : Function to get the series and model masking details using maskingName
        /// </summary>
        /// <param name="maskingName"></param>
        /// <returns></returns>
        SeriesMaskingResponse ProcessMaskingName(string maskingName);
	}
}
