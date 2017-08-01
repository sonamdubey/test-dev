using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates.UserReviews;
using Bikewale.Utility;
using BikewaleOpr.BAL;
using BikewaleOpr.DTO.UserReviews;
using BikewaleOpr.Entities.UserReviews;
using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.UserReviews;
using BikewaleOpr.Service.AutoMappers.UserReviews;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. kamble on 18 Apr 2017
    /// Summary : controller have methods related to user reviews api
    /// </summary>
    public class UserReviewsController : ApiController
    {
        private readonly IUserReviewsRepository _userReviewsRepo = null;

        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        /// <param name="userReviewsRepo"></param>
        public UserReviewsController(IUserReviewsRepository userReviewsRepo)
        {
            _userReviewsRepo = userReviewsRepo;
        }

        /// <summary>
        /// Function to update ther user reviews status. It can be approved or discarded. In case of disapproval need reason for disapproval.
        /// Modified by : Aditi Srivastava on 25 May 2017
        /// Summary     : Form user review detail link using new review id instead of old one to avoid redirection
        /// </summary>
        /// <param name="reviewId">User review id for which updation will happen</param>
        /// <param name="reviewStatus">Pass 2 for Approved or 3 for Discarded</param>
        /// <param name="disapprovalReasonId">If user review is discarded need reason id to discard review</param>
        /// <param name="moderatorId">Person opr user id who is updating the data</param>
        /// <returns></returns>
        [HttpPost, Route("api/userreviews/id/{reviewId}/updatestatus/")]
        public IHttpActionResult UpdateUserReviewsStatus(UpdateReviewsInputEntity inputs)
        {
            try
            {
                if (inputs != null && inputs.ReviewId > 0)
                {
                    uint oldTableReviewId = _userReviewsRepo.UpdateUserReviewsStatus(inputs.ReviewId, inputs.ReviewStatus, inputs.ModeratorId, inputs.DisapprovalReasonId, inputs.Review, inputs.ReviewTitle, inputs.ReviewTips, inputs.IsShortListed);

                    // Send mail to the user on approval or rejection
                    if (inputs.ReviewStatus.Equals(ReviewsStatus.Approved))
                    {
                        string reviewUrl = string.Format("{0}/{1}-bikes/{2}/reviews/{3}/", BWConfiguration.Instance.BwHostUrl, inputs.MakeMaskingName, inputs.ModelMaskingName, inputs.ReviewId);

                        ComposeEmailBase objBase = new ReviewApprovalEmail(inputs.CustomerName, reviewUrl, inputs.BikeName);
                        objBase.Send(inputs.CustomerEmail, "Congratulations! Your review has been published");
                    }
                    else if (inputs.ReviewStatus.Equals(ReviewsStatus.Discarded))
                    {
                        ComposeEmailBase objBase = new ReviewRejectionEmail(inputs.CustomerName, inputs.BikeName);
                        objBase.Send(inputs.CustomerEmail, "Oops! We request you to verify your review again");
                    }

                    if (inputs.ModelId > 0 && inputs.ReviewStatus != null)
                    {
                        //remove cache objects for reviews related to model
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_V1_{0}_CAT_1_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_V1_{0}_CAT_2_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_V1_{0}_CAT_5_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_V1_{0}_CAT_6_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_V1_{0}_CAT_7_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_BikeReviewsInfo_MO_{0}", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_BikeRatingsReviewsInfo_MO_V1_{0}", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ModelDetail_v1_{0}", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ReviewIdList_V1_{0}", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ReviewQuestionsValue_MO_", inputs.ModelId));
                    }

                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Service.Controllers.UserReviews.UpdateUserReviewsStatus");

                return InternalServerError();
            }

            return Ok(true);

        }   // End of UpdateUserReviewsStatus


        #region User Reviews Summary
        /// <summary>
        /// To get review Details 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns>Review Details</returns>
        [HttpGet, Route("api/userreviews/id/{reviewId}/summary/")]
        public IHttpActionResult GetUserReviewSummary(uint reviewId)
        {
            UserReviewSummary objUserReview = null;
            UserReviewSummaryDto objDTOUserReview = null;
            try
            {
                objUserReview = _userReviewsRepo.GetUserReviewSummary(reviewId);

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new UserReviewSummaryDto();
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    return Ok(objDTOUserReview);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController");
                return InternalServerError();
            }
            return NotFound();
        }   // Get review details
        #endregion

        [HttpGet, Route("api/userreviews/id/{reviewId}/email/{emailId}/summary/")]
        public IHttpActionResult GetUserReviewSummaryWithReviewIdEmailId(uint reviewId, string emailId)
        {
            UserReviewSummary objUserReview = null;
            UserReviewSummaryDto objDTOUserReview = null;
            try
            {
                objUserReview = _userReviewsRepo.GetUserReviewSummary(reviewId, emailId);

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new UserReviewSummaryDto();
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    return Ok(objDTOUserReview);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController");
                return InternalServerError();
            }
            return NotFound();
        }

        /// <summary>
        /// Created by Sajal Gupta on 19-06-2017
        /// Descrioption : Approve given comma separated review ids
        /// </summary>
        /// <param name="reviewIds"></param>
        /// <returns></returns>
        [HttpPost, Route("api/userreviews/ids/{reviewIds}/update/{status}")]
        public IHttpActionResult UpdateRatingStatus(string reviewIds, ReviewsStatus status, uint moderatedId)
        {
            bool updateStatus = false;
            try
            {
                //dispproval id- 6 means fake emailId/name
                updateStatus = _userReviewsRepo.UpdateUserReviewRatingsStatus(reviewIds, status, moderatedId, 6);

                if (status.Equals(ReviewsStatus.Approved))
                {
                    IEnumerable<BikeRatingApproveEntity> objReviewDetails = _userReviewsRepo.GetUserReviewDetails(reviewIds);

                    foreach(var obj in objReviewDetails)
                    {                                                
                        MemCachedUtil.Remove(string.Format("BW_BikeReviewsInfo_MO_{0}", obj.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_BikeRatingsReviewsInfo_MO_V1_{0}", obj.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ModelDetail_v1_{0}", obj.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ReviewIdList_V1_{0}", obj.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ReviewQuestionsValue_MO_", obj.ModelId));
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Service.Controllers.UserReviews.UpdateRatingStatus");
                return InternalServerError();
            }
            return Ok(updateStatus);
        }


    }   // class
}   // namespace
