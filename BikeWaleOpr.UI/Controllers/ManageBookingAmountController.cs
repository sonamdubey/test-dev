using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Interface;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.DealerBookingAmount;
using System;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created by : Vivek Singh Tomar On 5th Aug 2017
    /// Summary : Controller for ManageBookingAmountPage
    /// </summary>
    [Authorize]
    public class ManageBookingAmountController : Controller
    {
        private readonly IManageBookingAmountPage _manageBookingAmountPageData = null;
        private readonly ILocation _location = null;
        private readonly IDealers _dealersRepository = null;

        public ManageBookingAmountController(IManageBookingAmountPage manageBookingAmountPage, ILocation locationObject, IDealers dealersRepositoryObject)
        {
            _manageBookingAmountPageData = manageBookingAmountPage;
            _location = locationObject;
            _dealersRepository = dealersRepositoryObject;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar On 5th Aug 2017
        /// Summary : Get data model for manage booking amount page
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("dealers/{dealerId}/bookingamount/")]
        public ActionResult Index(uint dealerId, uint? cityId, uint? makeId, string dealerName = null)
        {
            DealerBookingAmountVM dealerBookingAmountInfo = null;
            DealerBookingAmountModel dealerBookingAmountModel = null;

            try
            {
                dealerBookingAmountModel = new DealerBookingAmountModel(_location, _dealersRepository, _manageBookingAmountPageData);
                dealerBookingAmountInfo = dealerBookingAmountModel.GetDealerBookingAmountData(dealerId, cityId.Value, makeId.Value, dealerName);
                if (TempData.ContainsKey("IsUpdated"))
                {
                    if ((bool)TempData["IsUpdated"])
                    {
                        dealerBookingAmountInfo.DealerBookingAmountData.UpdateMessage = "Booking amount updated";
                    }
                    else
                    {
                        dealerBookingAmountInfo.DealerBookingAmountData.UpdateMessage = "Failed to update booking amount";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.Controllers.ManageBookingAmountController dealer id {0}", dealerId));
            }
            return View(dealerBookingAmountInfo);
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar On 10th Aug 2017
        /// summary: Function to add/update booking amounts
        /// </summary>
        /// <param name="objBookingAmountEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(BookingAmountEntity objBookingAmountEntity)
        {
            try
            {
                bool isUpdated = _manageBookingAmountPageData.AddBookingAmount(objBookingAmountEntity, BikeWaleOpr.Common.CurrentUser.Id);
                TempData["IsUpdated"] = isUpdated;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.Controllers.ManageBookingAmountController.Add"));
            }
            return Redirect(string.Format("/dealers/{0}/bookingamount/?cityId={1}&makeId={2}&dealerName={3}",
                objBookingAmountEntity.DealerId,
                objBookingAmountEntity.CityId,
                objBookingAmountEntity.MakeId,
                objBookingAmountEntity.DealerName
                ));
        }
    }
}
