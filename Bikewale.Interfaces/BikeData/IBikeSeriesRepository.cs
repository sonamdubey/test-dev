using Bikewale.Entities.BikeData;
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
		IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId);
		BikeDescriptionEntity GetSynopsis(uint seriesId);
		IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId);
	}
}
