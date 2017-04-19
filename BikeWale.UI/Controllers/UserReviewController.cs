using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Models;
using Bikewale.Models.UserReviews;
using System;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UserReviewController : Controller
    {

        private readonly IUserReviews _userReviews = null;
        private IBikeModels<BikeModelEntity, int> _objModel = null;
        private readonly IUserReviewsRepository _userReviewsRepo = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bikeInfo"></param>
        /// <param name="userReviews"></param>
        public UserReviewController(IUserReviews userReviews, IBikeModels<BikeModelEntity, int> objModel, IUserReviewsRepository userReviewsRepo)
        {

            _userReviews = userReviews;
            _userReviewsRepo = userReviewsRepo;
            _objModel = objModel;
        }

        // GET: UserReview
        [Route("m/user-reviews/rate-bike/{modelId}/")]
        public ActionResult RateBike_Mobile(uint modelId, string reviewId)
        {
            UserReviewRatingPage objUserReview = new UserReviewRatingPage(modelId, _userReviews, _objModel, reviewId, _userReviewsRepo);
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();
            if (TempData["ErrorMessage"] != null)
            {
                UserReviewVM.ErrorMessage = Convert.ToString(TempData["ErrorMessage"]);
            }
            if (objUserReview != null)
            {
                if (objUserReview.status == Entities.StatusCodes.ContentFound)
                    UserReviewVM = objUserReview.GetData();
                else
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
            return View(UserReviewVM);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Action method to save user ratings
        /// </summary>
        /// <param name="overAllrating"></param>
        /// <param name="ratingQuestionAns"></param>
        /// <param name="userName"></param>
        /// <param name="emailId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [HttpPost, Route("user-reviews/ratings/save/"), ValidateAntiForgeryToken]
        public ActionResult SubmitRating(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint makeId, uint modelId, uint priceRangeId, uint reviewId)
        {

            bool isValid = true;
            string errorMessage = "";
            UserReviewRatingObject objRating = null;

            //server side validation for data received
            if (string.IsNullOrEmpty(overAllrating))
            {
                errorMessage = "Please provide your rating for bike.";
                isValid = false;
            }
            if (isValid && string.IsNullOrEmpty(ratingQuestionAns))
            {
                errorMessage = "Please rate all the questions.";
                isValid = false;
            }
            if (isValid && string.IsNullOrEmpty(userName))
            {
                errorMessage = "Please provide your username.";
                isValid = false;
            }
            if (isValid && string.IsNullOrEmpty(emailId))
            {
                errorMessage = "Please provide your Email Id.";
                isValid = false;
            }

            if (isValid)
            {
                objRating = _userReviews.SaveUserRatings(overAllrating, ratingQuestionAns, userName, emailId, makeId, modelId, 2, reviewId);

                string strQueryString = string.Format("reviewid={0}&makeid={1}&modelid={2}&overallrating={3}&customerid={4}&priceRangeId={5}&userName={6}&emailId={7}", objRating.ReviewId, makeId, modelId, overAllrating, objRating.CustomerId, priceRangeId, userName, emailId);

                string strEncoded = Utils.Utils.EncryptTripleDES(strQueryString);

                return Redirect("/m/write-a-review/?q=" + strEncoded);

                //return RedirectToAction("WriteReview_Mobile", new { strEncoded = strEncoded });
            }
            else
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("RateBike_Mobile");
            }
        }

        /// <summary>
        /// Created by Sajal Gupta on 10-04-2017
        /// Description : This action will fetch write review page.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [Route("m/user-reviews/write-review/")]
        public ActionResult WriteReview_Mobile(string q)
        {
            WriteReviewPageModel objPage = new WriteReviewPageModel(_userReviews, q);
            var objData = objPage.GetData();

            return View(objData);
        }

        [Route("m/user-reviews/review-summary/{reviewid}/")]
        public ActionResult ReviewSummary_Mobile(uint reviewid,string q)
        {
            UserReviewSummaryPage objData = new UserReviewSummaryPage(_userReviews, reviewid,q);
            UserReviewSummaryVM objVM = objData.GetData();
            return View(objVM);
        }


        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Action method to save user reviews
        /// </summary>
        /// <param name="reviewDescription"></param>
        /// <param name="reviewTitle"></param>
        /// <param name="reviewQuestion"></param>
        /// <param name="reviewTips"></param>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        [HttpPost, Route("user-reviews/save/"), ValidateAntiForgeryToken]
        public ActionResult SaveReview(string reviewDescription, string reviewTitle, string reviewQuestion, string reviewTips, string encodedId, string emailId, string userName, string makeName, string modelName, uint reviewId, string encodedString)
        {
            WriteReviewPageSubmitResponse objResponse = null;

            objResponse = _userReviews.SaveUserReviews(encodedId, reviewTips, reviewDescription, reviewTitle, reviewQuestion, emailId, userName, makeName, modelName, reviewDescription, reviewTitle);

            if (objResponse.IsSuccess)
                return Redirect(string.Format("/m/user-reviews/review-summary/{0}/?q={1}", reviewId, encodedString));
            else
            {
                WriteReviewPageModel objPage = new WriteReviewPageModel(_userReviews, encodedString);
                var objData = objPage.GetData();
                objData.SubmitResponse = objResponse;
                return View("WriteReview_Mobile", objData);
            }
        }

    }
}