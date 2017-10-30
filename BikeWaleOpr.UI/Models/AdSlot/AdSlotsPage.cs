using BikewaleOpr.Interface.AdSlot;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.AdSlot
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 30 Oct 2017
    /// Description : Provide methods to manage AdSlots.
    /// </summary>
    public class AdSlotsPage
    {
        private readonly IAdSlotRepository _AdSlotRepository = null;
        public AdSlotsPage(IAdSlotRepository AdSlotRepository)
        {
            _AdSlotRepository = AdSlotRepository;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 30 Oct 2017
        /// Description : Method to get data for Ad slot page.
        /// </summary>
        /// <returns></returns>
        public AdSlotVM GetData()
        {
            AdSlotVM objAdSlotVM = null;
            try
            {
                objAdSlotVM = new AdSlotVM();
                objAdSlotVM.UserId = Convert.ToUInt32(CurrentUser.Id);
                objAdSlotVM.AdSlotList = _AdSlotRepository.GetAdSlots();
            }
            catch (Exception ex)
            {
                BikeWaleOpr.Common.ErrorClass objErr = new BikeWaleOpr.Common.ErrorClass(ex, "AdSlotsPage.GetData");
            }
            return objAdSlotVM;
        }
    }
}