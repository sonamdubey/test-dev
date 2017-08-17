using Bikewale.Notifications;
using BikewaleOpr.Entities;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface;
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

        public ManageBookingAmountController(IManageBookingAmountPage manageBookingAmountPage)
        {
            _manageBookingAmountPageData = manageBookingAmountPage;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar On 5th Aug 2017
        /// Summary : Get data model for manage booking amount page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(UInt32 dealerId = 0)
        {
            ManageBookingAmountData objManageBookingAmountData = null;
            try
            {
                objManageBookingAmountData = _manageBookingAmountPageData.GetManageBookingAmountData(dealerId);
                objManageBookingAmountData.UpdateMessage = string.Empty;
                if (TempData.ContainsKey("IsUpdated"))
                {
                    if ((bool)TempData["IsUpdated"])
                    {
                        objManageBookingAmountData.UpdateMessage = "Booking amount updated";
                    }
                    else
                    {
                        objManageBookingAmountData.UpdateMessage = "Failed to update booking amount";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Controllers.ManageBookingAmountController dealer id {0}", dealerId));
            }
            return View(objManageBookingAmountData);
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar On 10th Aug 2017
        /// summary: Function to add/update booking amounts
        /// </summary>
        /// <param name="objBookingAmountEntity"></param>
        /// <returns></returns>
        public ActionResult Add(BookingAmountEntity objBookingAmountEntity)
        {
            try
            {
                bool isUpdated = _manageBookingAmountPageData.AddBookingAmount(objBookingAmountEntity, BikeWaleOpr.Common.CurrentUser.Id);
                if (isUpdated)
                {
                    TempData["IsUpdated"] = isUpdated;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Controllers.ManageBookingAmountController.Add"));
            }
            return RedirectToAction("/Index/", new { dealerId = objBookingAmountEntity.DealerId});
        }
    }
}