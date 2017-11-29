using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.Location
{
    /// <summary>
    /// Author      :   Sumit Kate on 25 Jan 2016
    /// Description :   Area Cache Repository
    /// </summary>
    public class AreaCacheRepository : IAreaCacheRepository
    {
        private readonly IDealerPriceQuote _dealerPQRepository = null;
        private readonly ICacheManager _cache = null;
        public AreaCacheRepository(ICacheManager cache, IDealerPriceQuote dealerPQRepository)
        {
            _cache = cache;
            _dealerPQRepository = dealerPQRepository;
        }
        /// <summary>
        /// Author      :   Sumit Kate on 25 Jan 2016
        /// Description :   Get the Area List by model id and city id from cache or DAL.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<Entities.Location.AreaEntityBase> GetAreaList(uint modelId, uint cityId)
        {
            IEnumerable<Entities.Location.AreaEntityBase> areaList = null;
            string key = string.Empty;
            try
            {
                key = String.Format("BW_PQArea_{0}_{1}", modelId, cityId);
                areaList = _cache.GetFromCache<IEnumerable<Entities.Location.AreaEntityBase>>(key, new TimeSpan(1, 0, 0), () => _dealerPQRepository.GetAreaList(modelId, cityId));
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikeModelsCacheRepository.GetModelPageDetails");
                
            }
            return areaList;
        }
    }
}
