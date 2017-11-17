using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.BikeData
{
    public class BikeSeriesCacheRepository : IBikeSeriesCacheRepository
    {
        private readonly ICacheManager _cache = null;
        private readonly IBikeSeriesRepository _bikeSeriesRepository = null;
        public BikeSeriesCacheRepository(ICacheManager cache, IBikeSeriesRepository bikeSeriesRepository)
        {
            _cache = cache;
            _bikeSeriesRepository = bikeSeriesRepository;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Sep 2017
        /// Summary : Cache for fetching models by series id
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public BikeSeriesModels GetModelsListBySeriesId(uint seriesId)
        {
            BikeSeriesModels objModels = null;
            string key = string.Format("BW_ModelsBySeriesId_{0}", seriesId);
            try
            {
                objModels = _cache.GetFromCache<BikeSeriesModels>(key, new TimeSpan(24, 0, 0), () => _bikeSeriesRepository.GetModelsListBySeriesId(seriesId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Cache.BikeData.BikeSeries.GetModelsListBySeries SeriesId = {0}", seriesId));

            }
            return objModels;

        }
        public IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId)
        {
            IEnumerable<BikeSeriesCompareBikes> Obj = null;
            string key = string.Format("BW_BikeSeriesComparision_{0}", seriesId);
            try
            {
                Obj = _cache.GetFromCache<IEnumerable<BikeSeriesCompareBikes>>(key, new TimeSpan(1, 0, 0), () => _bikeSeriesRepository.GetBikesToCompare(seriesId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Cache.BikeData.BikeSeries.GetBikesToCompare"));
            }

            return Obj;
        }
    }
}
