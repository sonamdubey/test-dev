using Bikewale.Notifications;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
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
        private readonly IBikeModels _objBikeModels = null;

        public UsedBikesController(ISellBikes objSellBikes, IBikeModels objBikeModels)
        {
            _objSellBikes = objSellBikes;
            _objBikeModels = objBikeModels;
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

        /// <summary>
        /// Created by : Sajal Gupta on 03-03-2017
        /// Description: Api to fetch photo id of used bike model image.
        /// </summary>
        /// <param name="objModelImageEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult FetchPhotoId([FromBody]UsedBikeModelImageEntity objModelImageEntity)
        {
            try
            {
                if (objModelImageEntity != null && ModelState.IsValid)
                {
                    return Ok(_objBikeModels.FetchPhotoId(objModelImageEntity));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("UsedBikesController.SaveBikeColorDetails objModelImageEntity {0}", objModelImageEntity));
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by : Sajal Gupta on 03-03-2017
        /// Description: Api to delete photo of used bike model image.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteUsedBikeModelImage(uint modelId)
        {
            try
            {
                if (modelId > 0)
                {
                    return Ok(_objBikeModels.DeleteUsedBikeModelImage(modelId));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("UsedBikesController.DeleteUsedBikeModelImage modelid {0}", modelId));
                return InternalServerError();
            }
        }
    }
}