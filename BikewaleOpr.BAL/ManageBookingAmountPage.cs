using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.BikeData;
using System;

namespace BikewaleOpr.BAL
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 5th Aug 2017
    /// Summary : BAL for manage booking amount page 
    /// </summary>
    public class ManageBookingAmountPage: IManageBookingAmountPage
    {
        private readonly IBikeMakesRepository _bikeMakesRepo = null;
        private readonly IDealers _dealers = null;

        public ManageBookingAmountPage(IBikeMakesRepository bikeMakesRepo, IDealers dealers)
        {
            _bikeMakesRepo = bikeMakesRepo;
            _dealers = dealers;
        }
        /// <summary>
        /// Created by : Vivek Singh Tomar On 5th Aug 2017
        /// Summary : Function to get manage booking amount page data
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public ManageBookingAmountData GetManageBookingAmountData(UInt32 dealerId)
        {
            ManageBookingAmountData objManageBookingAmountData = null;
            try
            {
                if(dealerId > 0)
                {
                    objManageBookingAmountData = new ManageBookingAmountData();
                    objManageBookingAmountData.BikeMakeList = _bikeMakesRepo.GetMakes(2);
                    objManageBookingAmountData.BookingAmountList = _dealers.GetBikeBookingAmount(dealerId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.BAL.ManageBookingAmountPage Dealer Id {0}", dealerId));
            }
            return objManageBookingAmountData;
        }

        public void AddBookingAmount(BookingAmountEntity objBookingAmountEntity, string updatedBy)
        {
            try
            {
                UInt32 updatedById = 0;
                UInt32.TryParse(updatedBy, out updatedById);
                if(objBookingAmountEntity.DealerId > 0 && objBookingAmountEntity.BikeModel.ModelId > 0 && 
                     objBookingAmountEntity.BookingAmountBase.Amount > 0 && updatedById > 0)
                {
                    _dealers.SaveBookingAmount(objBookingAmountEntity, updatedById);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.ManageBookingAmountPage.AddBookingAmount");
            }
        }
    }
}
