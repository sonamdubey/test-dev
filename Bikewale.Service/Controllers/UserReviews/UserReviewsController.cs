using Bikewale.DTO.UserReviews;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.UserReviews;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.BAL.UserReviews;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Utility.StringExtention;

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

        private readonly IUserReviewsRepository _userReviewsRepo = null;
        private readonly IUserReviews _userReviews = null;
        private readonly IUserReviewsCache _userReviewsCache = null;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userReviewsRepo"></param>
        /// <param name="userReviews"></param>
        /// <param name="userReviewsCache"></param>
        /// <param name="objBikeModel"></param>
        public UserReviewsController(IUserReviewsRepository userReviewsRepo, IUserReviews userReviews, IUserReviewsCache userReviewsCache)
        {
            _userReviewsRepo = userReviewsRepo;
            _userReviews = userReviews;
            _userReviewsCache = userReviewsCache;

        }

        #region User Reviews Details
        /// <summary>
        /// To get review Details 
        /// Modified by Sajal Gupta in 19-05-2017
        /// Description : Added logic update view count.
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
                    //update viewcount
                    Hashtable ht = _userReviewsCache.GetUserReviewsIdMapping();
                    if (ht.ContainsKey((int)reviewId))
                    {
                        uint newReviewId = Convert.ToUInt32(ht[(int)reviewId]);
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("par_reviewId", newReviewId.ToString());
                        SyncBWData.PushToQueue("updateUserReviewViews", DataBaseName.BW, nvc);
                    }

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

        /// <summary>
        /// Created by Sajal Gupta on 17-07-2017
        /// Description : Created api to increase user review view count
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpPost, Route("api/user-reviews/updateView/{reviewId}/")]
        public IHttpActionResult UpdateUserReviewViews(uint reviewId)
        {
            try
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("par_reviewId", reviewId.ToString());
                SyncBWData.PushToQueue("updateUserReviewViews", DataBaseName.BW, nvc);

                return Ok();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController.UpdateUserReviewViews");
                return InternalServerError();
            }
        }

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
        public IHttpActionResult Post(uint reviewId)
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
        public IHttpActionResult Post(uint reviewId, bool isHelpful)
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
        public IHttpActionResult Post(uint reviewId, string comment, string userId)
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

        #region User Reviews Summary
        /// <summary>
        /// To get review Details 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns>Review Details</returns>
        [ResponseType(typeof(ReviewDetails)), Route("api/user-reviews/summary/{reviewId}/")]
        public IHttpActionResult GetUserReviewSummary(uint reviewId)
        {
            UserReviewSummary objUserReview = null;
            UserReviewSummaryDto objDTOUserReview = null;
            try
            {
                objUserReview = _userReviews.GetUserReviewSummary(reviewId);

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

        #region User Reviews voting
        /// <summary>
        /// Created by Sajal Gupta on 05-05-2017
        /// Description : To vote user review 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="vote"></param>
        /// <returns></returns>
        [Route("api/user-reviews/voteUserReview/")]
        public IHttpActionResult VoteUserReview(uint reviewId, int vote)
        {
            try
            {
                if (vote == 0 || vote == 1)
                {
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("par_reviewId", reviewId.ToString());
                    nvc.Add("par_vote", vote.ToString());
                    SyncBWData.PushToQueue("VoteUserReview", DataBaseName.BW, nvc);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.VoteUserReview");
                return InternalServerError();
            }
        }
        #endregion

        #region User Reviews abuse
        /// <summary>
        /// Created by Sajal Gupta on 05-05-2017
        /// Description : Save abuse user review 
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        [HttpPost, Route("api/user-reviews/abuseUserReview/")]
        public IHttpActionResult SaveUserReviewAbuse(uint reviewId, string comments)
        {
            try
            {
                string ip = Bikewale.Utility.CurrentUser.GetClientIP();

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("par_reviewId", reviewId.ToString());
                nvc.Add("par_comments", comments);
                nvc.Add("par_ip", ip);
                SyncBWData.PushToQueue("SaveUserReviewAbuse", DataBaseName.BW, nvc);
                return Ok();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.SaveUserReviewAbuse");
                return InternalServerError();
            }
        }


        /// <summary>
        /// Created by Snehal Dange on 01-09-2017
        /// Description : This action will fetch rate bike page.
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/user-reviews/rate-bike/")]
        public IHttpActionResult RateBike([FromBody] RateBikeInput objRateBike)
        {
            try
            {
                RateBikeDetails objRateBikeDetails = null;
                UserReviewRatingData objReviewRatingData = null;
                if (objRateBike != null)
                {
                    
                    objReviewRatingData = _userReviews.GetRateBikeData(objRateBike.ModelId, objRateBike.ReviewId, objRateBike.CustomerId, objRateBike.SourceId, objRateBike.SelectedRating, objRateBike.IsFake, objRateBike.ReturnUrl, objRateBike.Contestsrc);
                    if (objReviewRatingData != null)
                    {
                        objRateBikeDetails = UserReviewsMapper.Convert(objReviewRatingData);
                        return Ok(objRateBikeDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController.RateBike");
                return InternalServerError();
            }
            return NotFound();
        }


        public RatingReviewInput SaveSubmitRatingObject(UserReviewRatingObject objRating, InputRatingSave objSaveInputRating)
        {
            RatingReviewInput objRatingReviewInput = new RatingReviewInput();

            objRatingReviewInput.CustomerId = objRating.CustomerId;
            objRatingReviewInput.ReviewId = objRating.ReviewId;
            objRatingReviewInput.IsFake = objRating.IsFake;

            objRatingReviewInput.ModelId = objSaveInputRating.ModelId;
            objRatingReviewInput.MakeId = objSaveInputRating.MakeId;
            objRatingReviewInput.PriceRangeId = objSaveInputRating.PriceRangeId;
            objRatingReviewInput.ReturnUrl = objSaveInputRating.ReturnUrl;
            objRatingReviewInput.SourceId = objSaveInputRating.SourceId;
            objRatingReviewInput.ContestSrc = objSaveInputRating.ContestSrc;
            objRatingReviewInput.EmailId = objSaveInputRating.EmailId;
            objRatingReviewInput.UserName = objSaveInputRating.UserName;
            objRatingReviewInput.OverAllrating = objSaveInputRating.OverAllrating;

            return objRatingReviewInput;
        }

        /// <summary>
        /// Created by Snehal Dange on 01-09-2017
        /// Description : This action will save the rating given by user
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/user-reviews/ratings/save/")]
        public IHttpActionResult SubmitRating([FromBody] InputRatingSave objSaveInputRating)
        {
            try
            {
                if(objSaveInputRating !=null)
                {
                    RatingReviewInput objRatingReviewInput = null;
                    UserReviewRatingObject objRating = null;
                    objSaveInputRating.UserName = StringHelper.ToProperCase(objSaveInputRating.UserName);

                    objRating = _userReviews.SaveUserRatings(objSaveInputRating.OverAllrating, objSaveInputRating.RatingQuestionAns , objSaveInputRating.UserName , objSaveInputRating.EmailId , objSaveInputRating.MakeId , objSaveInputRating.ModelId , objSaveInputRating.ReviewId , objSaveInputRating.ReturnUrl , objSaveInputRating.PlatformId , objSaveInputRating.utmzCookieValue, objSaveInputRating.SourceId );

                    objRatingReviewInput = SaveSubmitRatingObject(objRating, objSaveInputRating);
                    return Ok(objRatingReviewInput);
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.UserReviews.UserReviewsController.SubmitRating");
                return InternalServerError();
            }
            return NotFound();
        }
        #endregion

    }

}
