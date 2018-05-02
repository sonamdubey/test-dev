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
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Cache method to get new models of a series with city price.
        /// Modified by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Changed cache key from 'BW_NewModelsBySeriesId_s_{0}_c_{1}' to 'BW_NewModelsBySeriesId_seriesId_{0}_cityId_{1}'
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId)
        {
            IEnumerable<NewBikeEntityBase> objModels = null;
            try
            {
                string key = string.Format("BW_NewModelsBySeriesId_seriesId_{0}_cityId_{1}", seriesId, cityId);
                objModels = _cache.GetFromCache(key, new TimeSpan(6, 0, 0), () => _bikeSeriesRepository.GetNewModels(seriesId, cityId));
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Cache.BikeData.BikeSeries.GetNewModels_SeriesId_{0}_CityId_{1}", seriesId, cityId));
            }
            return objModels;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Cache method to get upcoming models of a series.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId)
        {
            IEnumerable<UpcomingBikeEntityBase> objModels = null;
            try
            {
                string key = string.Format("BW_UpcomingModelsBySeriesId_{0}", seriesId);
                objModels = _cache.GetFromCache(key, new TimeSpan(6, 0, 0), () => _bikeSeriesRepository.GetUpcomingModels(seriesId));
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Cache.BikeData.BikeSeries.GetUpcomingModels_SeriesId = {0}", seriesId));
            }
            return objModels;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Sep 2017
        /// Summary : Cache for fetching models by series id
        /// Modified by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Added call to GetNewModels, GetUpcomingModels to get models of a series.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public BikeSeriesModels GetModelsListBySeriesId(uint seriesId, uint cityId = 0)
        {
            BikeSeriesModels objModels = null;
            try
            {
                objModels = new BikeSeriesModels();
                objModels.NewBikes = GetNewModels(seriesId, cityId);
                objModels.UpcomingBikes = GetUpcomingModels(seriesId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Cache.BikeData.BikeSeries.GetModelsListBySeries SeriesId = {0}", seriesId));

            }
            return objModels;

        }
        public IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId)
        {
            IEnumerable<BikeSeriesCompareBikes> Obj = null;
            string key = string.Format("BW_BikeSeriesComparision_{0}", seriesId);
            try
            {
                Obj = _cache.GetFromCache(key, new TimeSpan(1, 0, 0), () => _bikeSeriesRepository.GetBikesToCompare(seriesId));
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Cache.BikeData.BikeSeries.GetBikesToCompare_SeriesId_{0}", seriesId));
            }

            return Obj;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Cache method to get synopsis of a series.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
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
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Cache.BikeData.BikeSeries.GetSynopsis_SeriesId_{0}", seriesId));
            }
            return synopsis;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : Cache method to get all series of a make.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
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
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Cache.BikeData.BikeSeries.GetOtherSeriesFromMake_makeId_{0}", makeId));
            }
            return bikeSeriesEntityList;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 20 Nov 2017
        /// Summary : Function to get the hashtable from cache and return response object for given maskingname
        /// </summary>
        /// <param name="maskingName">string value for which model/series details are required.</param>
        /// <returns>Returns model/series details associated with given masking name.</returns>
        public SeriesMaskingResponse ProcessMaskingName(string maskingName)
        {
            SeriesMaskingResponse objResponse = null;

            try
            {
                System.Collections.Hashtable objMaskingtable = _cache.GetFromCache<System.Collections.Hashtable>("BW_ModelSeries_MaskingNames", new TimeSpan(1, 0, 0, 0), () => _bikeSeriesRepository.GetMaskingNames());

                if (objMaskingtable != null && objMaskingtable.Contains(maskingName))
                {
                    objResponse = (SeriesMaskingResponse)objMaskingtable[maskingName];
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Cache.BikeData.BikeSeriesCacheRepository.ProcessMaskingName");
            }
            return objResponse;
        }

        /// <summary>
        /// created by : vivek singh tomar on 24th nov 2017
        /// summary : get modelids as comma separated string by series id
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public string GetModelIdsBySeries(uint seriesId)
        {
            string modelIds = string.Empty;
            try
            {
                string key = string.Format("BW_GetModelIdsBySeries_{0}", seriesId);
                modelIds = _cache.GetFromCache(key, new TimeSpan(1, 0, 0, 0), () => _bikeSeriesRepository.GetModelIdsBySeries(seriesId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Cache.BikeData.BikeSeriesCacheRepository.GetMaskingNames seriesId {0}", seriesId));
            }
            return modelIds;
        }


        /// <summary>
        /// Created By : Deepak Israni on 16 April 2018
        /// Description: To get the list of series by make with minimum price by city.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeSeriesEntity> GetMakeSeries(int makeId, int cityId)
        {
            IEnumerable<BikeSeriesEntity> makeSeriesEntityList = null;
            try
            {
                string key = (cityId == 0 ? string.Format("BW_MakeSeries_M_{0}", makeId) : string.Format("BW_MakeSeries_M_{0}_C_{1}", makeId, cityId));
                makeSeriesEntityList = _cache.GetFromCache(key, new TimeSpan(24, 0, 0), () => _bikeSeriesRepository.GetMakeSeries(makeId, cityId));
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Cache.BikeData.BikeSeries.GetMakeSeries_makeId_{0}_cityId_{1}", makeId, cityId));
            }
            return makeSeriesEntityList;
        }
    }
}
