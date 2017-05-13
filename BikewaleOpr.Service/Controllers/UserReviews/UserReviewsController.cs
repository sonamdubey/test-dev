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
                    uint oldTableReviewId = _userReviewsRepo.UpdateUserReviewsStatus(inputs.ReviewId, inputs.ReviewStatus, inputs.ModeratorId, inputs.DisapprovalReasonId, inputs.Review, inputs.ReviewTitle, inputs.ReviewTips);

                    // Send mail to the user on approval or rejection
                    if (inputs.ReviewStatus.Equals(ReviewsStatus.Approved) && oldTableReviewId > 0)
                    {
                        string reviewUrl = string.Format("{0}/{1}-bikes/{2}/user-reviews/{3}.html", BWConfiguration.Instance.BwHostUrl, inputs.MakeMaskingName, inputs.ModelMaskingName, oldTableReviewId);

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
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_{0}_CAT_1_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_{0}_CAT_2_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_{0}_CAT_5_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_{0}_CAT_6_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_UserReviews_MO_{0}_CAT_7_PN_1_PS_24", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_BikeReviewsInfo_MO_{0}", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_BikeRatingsReviewsInfo_MO_{0}", inputs.ModelId));
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


    }   // class
}   // namespace
