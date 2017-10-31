using Bikewale.Notifications;
using BikewaleOpr.Interface.AdSlot;
using System;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 30 Oct 2017
    /// Description : Api controller for ad slots.
    /// </summary>
    public class AdSlotController : ApiController
    {

        private readonly IAdSlot _adSlot = null;
        public AdSlotController(IAdSlot adSlot)
        {
            _adSlot = adSlot;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 30 Oct 2017
        /// Description : Api method to change status of ad slot.
        /// </summary>
        /// <param name="AdId">Id of Ad slot that need to be changed.</param>
        /// <param name="UserId">Id of User who is making changes.</param>
        /// <returns></returns>
        [HttpPost, Route("api/adslot/change/{AdId}/{UserId}/")]
        public IHttpActionResult ChangeStatusAdSlot(uint AdId, int UserId)
        {
            try
            {
                bool IsChanged = _adSlot.ChangeStatus(AdId, UserId);
                return Ok(IsChanged);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("AdSlotController.ChangeStatusAdSlot({0},{1})", AdId, UserId));
                return InternalServerError();
            }
        }
    }
}
