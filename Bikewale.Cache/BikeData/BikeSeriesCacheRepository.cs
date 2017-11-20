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

		public IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId)
		{
			IEnumerable<NewBikeEntityBase> objModels = null;
			string key = string.Format("BW_NewModelsBySeriesId_s_{0}_c_{1}", seriesId, cityId);
			try
			{
				objModels = _cache.GetFromCache(key, new TimeSpan(6, 0, 0), () => _bikeSeriesRepository.GetNewModels(seriesId, cityId));
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("Cache.BikeData.BikeSeries.GetNewModels_SeriesId = {0}", seriesId));
			}
			return objModels;
		}

		public IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId)
		{
			IEnumerable<UpcomingBikeEntityBase> objModels = null;
			string key = string.Format("BW_UpcomingModelsBySeriesId_{0}", seriesId);
			try
			{
				objModels = _cache.GetFromCache(key, new TimeSpan(6, 0, 0), () => _bikeSeriesRepository.GetUpcomingModels(seriesId));
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("Cache.BikeData.BikeSeries.GetUpcomingModels_SeriesId = {0}", seriesId));
			}
			return objModels;
		}
		/// <summary>
		/// Created by : Vivek Singh Tomar on 28th Sep 2017
		/// Summary : Cache for fetching models by series id
		/// </summary>
		/// <param name="seriesId"></param>
		/// <returns></returns>
		public BikeSeriesModels GetModelsListBySeriesId(uint seriesId, uint cityId = 0)
        {
            BikeSeriesModels objModels = null;
            string key = string.Format("BW_ModelsBySeriesId_{0}", seriesId);
            try
            {
				objModels = new BikeSeriesModels();
				objModels.NewBikes = GetNewModels(seriesId, cityId);
				objModels.UpcomingBikes = GetUpcomingModels(seriesId);
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

		public BikeDescriptionEntity GetSynopsis(uint seriesId)
		{
			BikeDescriptionEntity synopsis = null;
			try
			{
				string key = string.Format("BW_SynopsisBySeriesId_{0}", seriesId);
				synopsis = _cache.GetFromCache(key, new TimeSpan(6, 0, 0), () => _bikeSeriesRepository.GetSynopsis(seriesId));
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("Cache.BikeData.BikeSeries.GetSynopsis_SeriesId = {0}", seriesId));
			}
			return synopsis;
		}

		public IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId)
		{
			IEnumerable<BikeSeriesEntity> bikeSeriesEntityList = null;
			try
			{
				string key = string.Format("BW_OtherSeriesByMakeId_{0}", makeId);
				bikeSeriesEntityList = _cache.GetFromCache(key, new TimeSpan(6, 0, 0), () => _bikeSeriesRepository.GetOtherSeriesFromMake(makeId));
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("Cache.BikeData.BikeSeries.GetOtherSeriesFromMake_makeId_{0}", makeId));
			}
			return bikeSeriesEntityList;
		}
	}
}
