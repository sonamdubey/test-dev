using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bikewale.Notifications;
using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.UserReviews;

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
        [HttpPost, Route("userreviews/id/{reviewId}/updatestatus/")]
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

    }   // class
}   // namespace
