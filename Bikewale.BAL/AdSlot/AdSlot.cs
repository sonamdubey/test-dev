using Bikewale.Entities.Models;
using Bikewale.Interfaces.AdSlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.BAL.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Provide BAL methods for ad slots.
    /// </summary>
    public class AdSlot : IAdSlot
    {

        private readonly IAdSlotCacheRepository _adSlotCache;
        public AdSlot(IAdSlotCacheRepository adSlotCache)
        {
            _adSlotCache = adSlotCache;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 31 Oct 2017
        /// Description : BAL method to check ad slot status.
        /// </summary>
        /// <param name="adSlotName">Ad slot name ex. Ad_292x399</param>
        /// <returns></returns>
        public bool CheckAdSlotStatus(string adSlotName)
        {
            bool IsActive = false;
            try
            {
                IEnumerable<AdSlotEntity> objAdSlotList = _adSlotCache.GetAdSlotStatus();
                if (objAdSlotList != null && objAdSlotList.Any())
                {
                    IsActive = objAdSlotList.Any(a => a.Name == adSlotName && a.Status);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "BAL.AdSlot.CheckAdSlotStatus");
            }
            return IsActive;
        }
    }
}
