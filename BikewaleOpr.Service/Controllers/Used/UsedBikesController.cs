using Bikewale.Notifications;
using BikewaleOpr.Interface.Used;
using System;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.Used
{
    /// <summary>
    /// Created by : Aditi Srivastava on 24 Oct 2016
    /// Summary    : Controller for used bikes
    /// </summary>
    public class UsedBikesController : ApiController
    {
        private readonly ISellBikes _objSellBikes = null;

        public UsedBikesController(ISellBikes objSellBikes)
        {
            _objSellBikes = objSellBikes;
        }

        /// <summary>
        ///  Created by : Aditi Srivastava on 24 Oct 2016
        ///  Summary    : To update edited entries
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="isApproved"></param>
        /// <param name="approvedBy"></param>
        /// <returns></returns>
        [HttpPost, Route("api/used/sell/pendinginquiries/{inquiryId}/")]
        public IHttpActionResult SaveEditedInquiry(uint inquiryId, short isApproved, int approvedBy, string profileId, string bikeName)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = _objSellBikes.SaveEditedInquiry(inquiryId, isApproved, approvedBy, profileId, bikeName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("UsedBikesController.SaveEditedInquiry: InquiryId:{0}, IsApproved:{1}, Approvedby{2}, profileid {3}", inquiryId, isApproved, approvedBy, profileId));
                objErr.SendMail();
                return InternalServerError();
            }
            return Ok(isSuccess);
        }
    }
}