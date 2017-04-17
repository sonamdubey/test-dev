﻿using Bikewale.Entities.BikeData;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bikeInfo"></param>
        /// <param name="userReviews"></param>
        public UserReviewController(IUserReviews userReviews, IBikeModels<BikeModelEntity, int> objModel)
        {

            _userReviews = userReviews;
            _objModel = objModel;
        }

        // GET: UserReview
        [Route("m/user-reviews/rate-bike/{modelId}")]
        public ActionResult RateBike_Mobile(uint modelId, uint? reviewId)
        {
            UserReviewRatingPage objUserReview = new UserReviewRatingPage(modelId, _userReviews, _objModel, reviewId);
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();
            if (TempData["ErrorMessage"] != null)
            {
                UserReviewVM.ErrorMessage = Convert.ToString(TempData["ErrorMessage"]);
            }
            UserReviewVM = objUserReview.GetData();

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
        public ActionResult SubmitRating(string overAllrating, string ratingQuestionAns, string userName, string emailId, uint makeId, uint modelId)
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
                objRating = _userReviews.SaveUserRatings(overAllrating, ratingQuestionAns, userName, emailId, makeId, modelId);

                string strQueryString = string.Format("reviewid={0}&makeid={1}&modelid={2}&overallrating={3}&customerid={4}", objRating.ReviewId, makeId, modelId, overAllrating, objRating.CustomerId);

                string strEncoded = Utils.Utils.EncryptTripleDES(strQueryString);

                return RedirectToAction("WriteReview_Mobile", new { strEncoded = strEncoded });
            }
            else
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("RateBike_Mobile");
            }
        }


        [Route("m/user-reviews/write-review/")]
        public ActionResult WriteReview_Mobile(string strEncoded)
        {
            WriteReviewPageModel objPage = new WriteReviewPageModel(_userReviews, strEncoded);
            var objData = objPage.GetData();

            return View(objData);
        }

        [Route("m/user-reviews/review-summary/")]
        public ActionResult ReviewSummary_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
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
        public ActionResult SaveReview(string reviewDescription, string reviewTitle, string reviewQuestion, string reviewTips, uint reviewId)
        {
            bool isValid = true;
            string errorMessage = "";
            //server side validation for data received
            if (string.IsNullOrEmpty(reviewDescription) && !string.IsNullOrEmpty(reviewTitle))
            {
                errorMessage = "Please provide your Description for bike.";
                isValid = false;
            }
            if (!string.IsNullOrEmpty(reviewDescription) && string.IsNullOrEmpty(reviewTitle))
            {
                errorMessage = "Please provide your Title for bike.";
                isValid = false;
            }

            if (isValid)
            {
                _userReviews.SaveUserReviews(reviewId, reviewTips, reviewDescription, reviewTitle, reviewQuestion);
                return RedirectToAction("ReviewSummary_Mobile");
            }
            else
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("WriteReview_Mobile");
            }
        }

    }
}