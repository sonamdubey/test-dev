using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.UserReviews;
using Bikewale.Service.Utilities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.UserReviews
{
    /// <summary>
    /// To Get Detailed User Reviews
    /// And To Update Review Status to Helpful/Abuse/ViewCount
    /// Author : Sushil Kumar
    /// Created On : 26th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class UserReviewsController : CompressionApiController//ApiController
    {

        private readonly IUserReviews _userReviewsRepo = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userReviewsRepo"></param>
        public UserReviewsController(IUserReviews userReviewsRepo)
        {
            _userReviewsRepo = userReviewsRepo;
        }

        #region User Reviews Details
        /// <summary>
        /// To get review Details 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns>Review Details</returns>
        [ResponseType(typeof(ReviewDetails))]
        public IHttpActionResult Get(uint reviewId)
        {
            ReviewDetailsEntity objUserReview = null;
            ReviewDetails objDTOUserReview = null;
            try
            {
                objUserReview = _userReviewsRepo.GetReviewDetails(reviewId);

                if (objUserReview != null)
                {
                    // Auto map the properties
                    objDTOUserReview = new ReviewDetails();
                    objDTOUserReview = UserReviewsMapper.Convert(objUserReview);

                    return Ok(objUserReview);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }   // Get review details
        #endregion

        #region Get User Review Ratings
        /// <summary>
        /// To get Review Ratings based on Model Id
        /// </summary>
        /// <param name="modelId">Model Id should be positive</param>
        /// <param name="review">Optional</param>
        /// <returns>User Review rating</returns>
        [ResponseType(typeof(ReviewRating))]
        public IHttpActionResult Get(uint modelId, bool? review)
        {
            ReviewRatingEntity objURRating = null;
            ReviewRating objDTOURRating = null;
            try
            {
                objURRating = _userReviewsRepo.GetBikeRatings(modelId);

                if (objURRating != null)
                {
                    // Auto map the properties
                    objDTOURRating = new ReviewRating();
                    objDTOURRating = UserReviewsMapper.Convert(objURRating);

                    return Ok(objURRating);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }   // Get user review ratings
        #endregion

        #region Update Reviews View Count
        /// <summary>
        /// Update Reviews View Count for reviewId
        /// </summary>
        /// <param name="reviewId">Should be Positive</param>
        /// <returns>Boolean</returns>
        [ResponseType(typeof(Boolean))]
        public IHttpActionResult Put(uint reviewId)
        {
            bool objURRating = false;
            try
            {
                objURRating = _userReviewsRepo.UpdateViews(reviewId);

                if (objURRating)
                {
                    return Ok(objURRating);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController");
                objErr.SendMail();
                return InternalServerError();
            }

            return (IHttpActionResult)Request.CreateResponse(HttpStatusCode.NotModified, "Oops ! Something Went Wrong");
        }   // Update user review views count
        #endregion

        #region Is Helpful  User Review
        /// <summary>
        ///  To Change review status to helpful
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="isHelpful">Optional (use as '&isHelpful')</param>
        /// <returns>Boolean</returns>
        [ResponseType(typeof(Boolean))]
        public IHttpActionResult Put(uint reviewId, bool isHelpful)
        {
            bool objURHelpful = false;
            try
            {
                objURHelpful = _userReviewsRepo.UpdateReviewUseful(reviewId, isHelpful);

                if (objURHelpful)
                {
                    return Ok(objURHelpful);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController");
                objErr.SendMail();
                return InternalServerError();
            }
            return (IHttpActionResult)Request.CreateResponse(HttpStatusCode.NotModified, "Oops ! Something Went Wrong");
        }   // Update is Helpful 
        #endregion

        #region Abuse User Review
        /// <summary>
        /// To Update isAbuse status of review
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="comment"></param>
        /// <param name="userId"></param>
        /// <returns>Boolean</returns>
        [ResponseType(typeof(Boolean))]
        public IHttpActionResult Put(uint reviewId, string comment, string userId)
        {
            bool objURAbuse = false;
            try
            {
                objURAbuse = _userReviewsRepo.AbuseReview(reviewId, comment, userId);

                if (objURAbuse)
                {
                    return Ok(objURAbuse);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController");
                objErr.SendMail();
                return InternalServerError();
            }
            return (IHttpActionResult)Request.CreateResponse(HttpStatusCode.NotModified, "Oops ! Something Went Wrong");
        }   // Upadate Isabuse
        #endregion
    }
}
