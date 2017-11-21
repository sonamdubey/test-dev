using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : BAL interface for bike series
    /// </summary>
    public interface IBikeSeriesRepository
    {
        //BikeSeriesModels GetModelsListBySeriesId(uint seriesId);
		IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId);
        IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId);
		IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId);
		BikeDescriptionEntity GetSynopsis(uint seriesId);
		IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId);

        /// <summary>
        /// Written By : Ashish G. Kamble on 20 Nov 2017
        /// Summary : Function to get series and model masking names and details associated with it in hashtable
        /// </summary>
        /// <returns></returns>
        System.Collections.Hashtable GetMaskingNames();

    }
}
