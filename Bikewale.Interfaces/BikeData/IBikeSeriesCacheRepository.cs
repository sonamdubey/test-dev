using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
	/// <summary>
	/// Created by : Vivek Singh Tomar on 28th Sep 2017
	/// Summary : Cache Repo interface for bike series
	/// Modified by : Ashutosh Sharma on 17 Nov 2017
	/// Description : Added GetNewModels, GetUpcomingModels, GetOtherSeriesFromMake, GetSynopsis.
	/// </summary>
	public interface IBikeSeriesCacheRepository
    {
        BikeSeriesModels GetModelsListBySeriesId(uint seriesId, uint cityId = 0);
		IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId);
        IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId);
		IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId);
		BikeDescriptionEntity GetSynopsis(uint seriesId);
		IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId);
	}
}
