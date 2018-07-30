using System.Collections.Generic;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : BAL interface for bike series
    /// Modified by : Ashutosh Sharma on 17 Nov 2017
    /// Description : Added GetNewModels, GetUpcomingModels, GetOtherSeriesFromMake, GetSynopsis, Removed GetModelsListBySeriesId.
    /// Modified by : Vivek Singh Tomar on 24th Nov 2017
    /// Summary : added GetModelIdsBySeries
    /// Modified By : Deepak Israni on 16 April 2018
    /// Description : Added GetMakeSeries
    /// </summary>
    public interface IBikeSeriesRepository
    {
		IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId);
        IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId);
		IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId);
		BikeDescriptionEntity GetSynopsis(uint seriesId);
		IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId);
        string GetModelIdsBySeries(uint seriesId);
        IEnumerable<BikeSeriesEntity> GetMakeSeries(int makeId, int cityId);
        /// <summary>
        /// Written By : Ashish G. Kamble on 20 Nov 2017
        /// Summary : Function to get series and model masking names and details associated with it in hashtable
        /// </summary>
        /// <returns></returns>
        System.Collections.Hashtable GetMaskingNames();

    }
}
