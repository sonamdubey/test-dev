using Bikewale.Entities.Models;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Provide cache methods for ad slots.
    /// </summary>
    public class AdSlotCacheRepository : IAdSlotCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IAdSlotRepository _adSlotRepository;
        public AdSlotCacheRepository(ICacheManager cache, IAdSlotRepository adSlotRepository)
        {
            _cache = cache;
            _adSlotRepository = adSlotRepository;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Cache repository method to get status of ad slots, Cache of 7 days.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdSlotEntity> GetAdSlotStatus()
        {
            IEnumerable<AdSlotEntity> objAdSlotList = null;
            try
            {
                string key = "BW_AdSlots";
                objAdSlotList = _cache.GetFromCache(key, new TimeSpan(7, 0, 0, 0), () => _adSlotRepository.GetAdSlotStatus());
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "AdSlotCacheRepository.GetAdSlotStatus");
            }
            return objAdSlotList;
        }
    }
}
