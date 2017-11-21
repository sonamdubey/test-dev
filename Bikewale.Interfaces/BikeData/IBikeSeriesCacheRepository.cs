﻿using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using System.Collections.Generic;

namespace Bikewale.Interfaces.BikeData
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 28th Sep 2017
    /// Summary : Cache Repo interface for bike series
    /// </summary>
    public interface IBikeSeriesCacheRepository
    {
        BikeSeriesModels GetModelsListBySeriesId(uint seriesId, uint cityId = 0);
		IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId);
        IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId);
		IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId);
		BikeDescriptionEntity GetSynopsis(uint seriesId);
		IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId);

        /// <summary>
        /// Written By : Ashish G. Kamble on 20 Nov 2017
        /// Summary : Function to get the series and model masking details using maskingName
        /// </summary>
        /// <param name="maskingName"></param>
        /// <returns></returns>
        SeriesMaskingResponse ProcessMaskingName(string maskingName);
	}
}
