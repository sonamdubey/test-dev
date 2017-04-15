using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Models;
using Bikewale.Models.UserReviews;
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
            UserReviewVM = objUserReview.GetData();
            return View(UserReviewVM);
        }

        [HttpPost, Route("user-reviews/ratings/save/")]
        public ActionResult SubmitRating(string overAllrating, string ratingQuestionAns, string userName, string emailId)
        {
            return View();
        }


        [Route("m/user-reviews/write-review/")]
        public ActionResult WriteReview_Mobile()
        {
            WriteReviewPageVM objPage = new WriteReviewPageModel().GetData();

            return View(objPage);
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