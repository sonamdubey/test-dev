using Bikewale.Notifications;
using BikewaleOpr.Cache;
using BikewaleOpr.Interface.AdSlot;
using System;

namespace BikewaleOpr.BAL.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Provide BAL methods for ad slots.
    /// </summary>
    public class AdSlot : IAdSlot
    {
        private readonly IAdSlotRepository _AdSlotRepository = null;
        public AdSlot(IAdSlotRepository AdSlotRepository)
        {
            _AdSlotRepository = AdSlotRepository;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 31 Oct 2017
        /// Description : BAL method to chagne ad slot status.
        /// </summary>
        /// <param name="AdId">Status to be changed of Ad with AdId</param>
        /// <param name="UserId">User who is changing.</param>
        /// <returns></returns>
        public bool ChangeStatus(uint AdId, int UserId)
        {
            bool IsChanged = false;
            try
            {
                IsChanged = _AdSlotRepository.ChangeStatus(AdId, UserId);
                if (IsChanged)
                {
                    BwMemCache.ClearAdSlotsCache();
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, String.Format("AdSlot.ChangeStatus({0},{1})", AdId, UserId));

            }
            return IsChanged;
        }
    }
}
