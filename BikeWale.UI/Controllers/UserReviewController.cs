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
        private readonly IBikeInfo _bikeInfo = null;
        private readonly IUserReviews _userReviews = null;

        public UserReviewController(IBikeInfo bikeInfo, IUserReviews userReviews)
        {
            _bikeInfo = bikeInfo;
            _userReviews = userReviews;
        }

        // GET: UserReview
        [Route("m/user-reviews/rate-bike/{modelId}")]
        public ActionResult RateBike_Mobile(uint modelId)
        {
            UserReviewRatingPage objUserReview = new UserReviewRatingPage(modelId, _bikeInfo, _userReviews);
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();
            if (TempData["ErrorMessage"] != null)
            {
                UserReviewVM.ErrorMessage = Convert.ToString(TempData["ErrorMessage"]);
            }

            UserReviewVM = objUserReview.GetData();

            return View(UserReviewVM);
        }

        [HttpPost, Route("user-reviews/ratings/save/")]
        public ActionResult SubmitRating(string overAllrating, string ratingQuestionAns, string userName, string emailId)
        {
            bool isValid = false;
            string errorMessage = "";
            //server side validation for data received
            if (!string.IsNullOrEmpty(overAllrating))
            {
                errorMessage = "Please provide your rating for bike.";
                isValid = true;
            }
            if (!isValid && !string.IsNullOrEmpty(ratingQuestionAns))
            {
                errorMessage = "Please rate all the questions.";
                isValid = true;
            }
            if (!isValid && !string.IsNullOrEmpty(userName))
            {
                errorMessage = "Please provide your username.";
                isValid = true;
            }
            if (!isValid && !string.IsNullOrEmpty(emailId))
            {
                errorMessage = "Please provide your Email Id.";
                isValid = true;
            }

            if (isValid)
            {
                return RedirectToAction("WriteReview_Mobile");
            }
            else
            {
                TempData["ErrorMessage"] = errorMessage;
                return RedirectToAction("RateBike_Mobile");
            }
        }


        [Route("m/user-reviews/write-review/")]
        public ActionResult WriteReview_Mobile()
        {
            WriteReviewPageModel objPage = new WriteReviewPageModel(_userReviews);
            var objData = objPage.GetData();

            return View(objData);
        }

        [Route("m/user-reviews/review-summary/")]
        public ActionResult ReviewSummary_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }


        [HttpPost, Route("user-reviews/save/")]
        public void SaveReview(string reviewDescription, string reviewTitle, string reviewQuestion, string reviewTips)
        {

        }

    }
}