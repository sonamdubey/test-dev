using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates.UserReviews;
using Bikewale.Utility;
using BikewaleOpr.BAL;
using BikewaleOpr.DTO.UserReviews;
using BikewaleOpr.Entities.UserReviews;
using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.BikeData;
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
        private readonly IBikeModels _bikeModels = null;

        /// <summary>
        /// Constructor to initialize dependencies
        /// </summary>
        /// <param name="userReviewsRepo"></param>
        public UserReviewsController(IUserReviewsRepository userReviewsRepo, IBikeModels bikeModels)
        {
            _userReviewsRepo = userReviewsRepo;
            _bikeModels = bikeModels;
        }

        /// <summary>
        /// Function to update ther user reviews status. It can be approved or discarded. In case of disapproval need reason for disapproval.
        /// Modified by : Aditi Srivastava on 25 May 2017
        /// Summary     : Form user review detail link using new review id instead of old one to avoid redirection
        /// Modified by : Vivek Singh Tomar on 27th Sep 2017
        /// Summary : Changed version of cache key
        /// Modified by : Ashutosh Sharma on 04 Oct 2017
        /// Description : Changed cacke key from 'BW_ModelDetail_' to 'BW_ModelDetail_V1'.
        /// Modified :Snehal Dange on 6th Nov 2017
        /// Description: Added logic for refreshing cache key "BW_BikesByMileage";
        /// Modified :Snehal Dange on 21st Nov 2017
        /// Description: Added logic for refreshing cache key "BW_PopularBikesWithRecentAndHelpfulReviews_Make_";
        /// Modified by : Rajan Chauhan on 06 Feb 2018.
        /// Description : Changed version of key from 'BW_ModelDetail_V1_' to 'BW_ModelDetail_'.
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
                        MemCachedUtil.Remove(string.Format("BW_ModelDetail_{0}", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ReviewIdList_V1_{0}", inputs.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ReviewQuestionsValue_MO_", inputs.ModelId));
                        MemCachedUtil.Remove("BW_RecentReviews");
                        MemCachedUtil.Remove("BW_UserReviewIdMapping");
                        MemCachedUtil.Remove("BW_BikesByMileage");
                        if(inputs.MakeId > 0)
                        {
                            MemCachedUtil.Remove(string.Format("BW_PopularBikesWithRecentAndHelpfulReviews_Make_{0}", inputs.MakeId));
                        }

                        if (inputs.ReviewStatus.Equals(ReviewsStatus.Approved))
                        {
                            _bikeModels.UpdateModelESIndex(Convert.ToString(inputs.ModelId), "update");
                        }
                    }

                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.UserReviews.UpdateUserReviewsStatus");

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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController");
                return InternalServerError();
            }
            return NotFound();
        }   // Get review details
        #endregion       

        /// <summary>
        /// Created by Sajal Gupta on 19-06-2017
        /// Descrioption : Approve given comma separated review ids
        /// Modified by : Vivek Singh Tomar on 27th Sep 2017
        /// Summary : Changed version of cache key
        /// Modified by : Ashutosh Sharma on 04 Oct 2017
        /// Description : Changed cacke key from 'BW_ModelDetail_' to 'BW_ModelDetail_V1'.
        /// Modified by:Snehal Dange on 6th Nov 2017
        /// Description: Added logic for refreshing cache key "BW_BikesByMileage";
        /// Modified by : Rajan Chauhan on 06 Feb 2018.
        /// Description : Changed version of key from 'BW_ModelDetail_V1_' to 'BW_ModelDetail_'.
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

                    string updatedIds = string.Empty;

                    foreach(var obj in objReviewDetails)
                    {                                                
                        MemCachedUtil.Remove(string.Format("BW_BikeReviewsInfo_MO_{0}", obj.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_BikeRatingsReviewsInfo_MO_V1_{0}", obj.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ModelDetail_{0}", obj.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ReviewIdList_V1_{0}", obj.ModelId));
                        MemCachedUtil.Remove(string.Format("BW_ReviewQuestionsValue_MO_{0}", obj.ModelId));
                        updatedIds += string.Format("{0},", obj.ModelId);
                    }
                    MemCachedUtil.Remove("BW_UserReviewIdMapping");
                    MemCachedUtil.Remove("BW_BikesByMileage");


                    _bikeModels.UpdateModelESIndex(updatedIds, "update");
                }
            }
            catch(Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.UserReviews.UpdateRatingStatus");
                return InternalServerError();
            }
            return Ok(updateStatus);
        }

        /// <summary>
        /// Created by sajal Gupta on 01-08-2017
        /// Descriptiopin : Api to save user review winner
        /// Modified by: Vivek Singh Tomar on 12th Aug 2017
        /// Summary: Clear Cache when new winner added
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="moderatedId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/userreview/markwinner/id/{reviewId}/{moderatedId}/")]
        public IHttpActionResult SaveUserReviewWinner(uint reviewId, uint moderatedId)
        {
            bool updateStatus = false;
            try
            {                
                updateStatus = _userReviewsRepo.SaveUserReviewWinner(reviewId, moderatedId);
                if (updateStatus)
                {
                    //Clear user review contest winners cache when new winner added
                    MemCachedUtil.Remove("BW_UserReviewsWinners");
                }               
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "BikewaleOpr.Service.Controllers.UserReviews.UpdateRatingStatus");
                return InternalServerError();
            }
            return Ok(updateStatus);
        }

    }   // class
}   // namespace
