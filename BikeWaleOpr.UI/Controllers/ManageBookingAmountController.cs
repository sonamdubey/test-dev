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
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Controllers.ManageBookingAmountController dealer id {0}", dealerId));
            }
            return View(objManageBookingAmountData);
        }

        public ActionResult Add(uint dealerId, int modelId, uint bookingAmount, int versionId = 0, uint bookingId = 0)
        {
            try
            {
                BookingAmountEntity objBookingAmountEntity = new BookingAmountEntity()
                {
                    BookingAmountBase = new BookingAmountEntityBase()
                    {
                        Id = bookingId,
                        Amount = bookingAmount
                    },
                    BikeModel = new BikeModelEntityBase()
                    {
                        ModelId = modelId
                    },
                    BikeVersion = new BikeVersionEntityBase()
                    {
                        VersionId = versionId
                    },
                    UpdatedOn = DateTime.Now,
                    DealerId = dealerId
                };
                _manageBookingAmountPageData.AddBookingAmount(objBookingAmountEntity, BikeWaleOpr.Common.CurrentUser.Id);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Controllers.ManageBookingAmountController.Add"));
            }
            return RedirectToAction("/Index", new { dealerId = dealerId});
        }
    }
}