using Bikewale.Entities.BikeData;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.UsedBikes
{
    /// <summary>
    /// Written By : Sajal Gupta on 14/09/2016
    /// Description : Get bikes based on model/ make/city .
    /// </summary>
    public class UsedBikesCache : IUsedBikesCache
    {
        private readonly ICacheManager _cache;
        private readonly IUsedBikes _objModels;

        /// <summary>
        /// Written by : Sajal Gupta on 14/09/2016
        /// Intitalize the references for the cache and DL
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="objModels"></param>
        public UsedBikesCache(ICacheManager cache, IUsedBikes objModels)
        {
            _cache = cache;
            _objModels = objModels;
        }

        /// <summary>
        /// Written By : Sajal Gupta on 14/09/2016
        /// Description : Get bikes based on model/ make/city .
        /// </summary>
        public IEnumerable<MostRecentBikes> GetUsedBikes(uint makeId, uint modelId, uint cityId, uint totalCount)
        {
            IEnumerable<MostRecentBikes> objUsedBikes = null;

            string key = (modelId == 0) ? String.Format("BW_MostRecentUsedBikes_Make_{0}", makeId) : String.Format("BW_MostRecentUsedBikes_Model_{0}", modelId);
            if (cityId != 0)
                key = String.Format("{0}_CityId_{1}", key, cityId);

            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<MostRecentBikes>>(Convert.ToString(key), new TimeSpan(0, 30, 0), () => _objModels.GetPopularUsedBikes(makeId, modelId, cityId, totalCount));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception in Bikewale.Cache.UsedBikes.GetUsedBikes parametres makeId : {0}, modelId : {1}, cityId : {2}, totalCount : {3}", makeId, modelId, cityId, totalCount));

            }
            return objUsedBikes;

        }

        /// <summary>
        /// Written By : Sajal Gupta on 14/09/2016
        /// Description : Get bikes based on model/ make/city .
        /// </summary>
        public IEnumerable<MostRecentBikes> GetUsedBikesSeries(uint seriesid, uint cityId)
        {
            IEnumerable<MostRecentBikes> objUsedBikes = null;

            string key = String.Format("BW_MostRecentUsedBikesSeries_Model_{0}", seriesid);
            if (cityId != 0)
                key = String.Format("{0}_CityId_{1}", key, cityId);

            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<MostRecentBikes>>(Convert.ToString(key), new TimeSpan(0, 30, 0), () => _objModels.GetUsedBikesSeries(seriesid, cityId));
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, String.Format("Exception in Bikewale.Cache.UsedBikes.GetUsedBikesSeries parametres  modelId : {0}, cityId : {1}", seriesid, cityId));

            }
            return objUsedBikes;

        }



        /// <summary>
        /// Created by: Sangram Nandkhile on 06 oct 2016
        /// Summary: Cache the make and used bike counts for make in India
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsedBikeMakeEntity> GetUsedBikeMakesWithCount()
        {
            IEnumerable<UsedBikeMakeEntity> objUsedBikes = null;

            string key = "BW_UsedBikesMakeWithCount";
            try
            {
                objUsedBikes = _cache.GetFromCache<IEnumerable<UsedBikeMakeEntity>>(Convert.ToString(key), new TimeSpan(1, 0, 0), () => _objModels.GetUsedBikeMakesWithCount());
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception in Bikewale.Cache.UsedBikes.GetUsedBikeMakesWithCount");
                
            }
            return objUsedBikes;
        }
    }
}
