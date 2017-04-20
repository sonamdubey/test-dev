using Bikewale.Notifications;
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
        public IHttpActionResult UpdateUserReviewsStatus(uint reviewId, ReviewsStatus reviewStatus, uint moderatorId, ushort disapprovalReasonId, string review, string reviewTitle, string reviewTips)
        {
            try
            {
                _userReviewsRepo.UpdateUserReviewsStatus(reviewId, reviewStatus, moderatorId, disapprovalReasonId, review, reviewTitle, reviewTips);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Service.Controllers.UserReviews.UpdateUserReviewsStatus");

                return InternalServerError();
            }

            return Ok();

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
