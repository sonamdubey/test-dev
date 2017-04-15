using Bikewale.Interfaces.BikeData;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UserReviewController : Controller
    {
        private readonly IBikeInfo _bikeInfo = null;

        public UserReviewController(IBikeInfo bikeInfo)
        {
            _bikeInfo = bikeInfo;
        }

        // GET: UserReview
        [Route("m/user-reviews/rate-bike/{modelId}")]
        public ActionResult RateBike_Mobile(uint modelId)
        {
            UserReviewRating objUserReview = new UserReviewRating(modelId, _bikeInfo);
            UserReviewRatingVM UserReviewVM = new UserReviewRatingVM();
            UserReviewVM = objUserReview.GetData();
            return View(UserReviewVM);
        }

        [HttpPost, Route("user-reviews/ratings/save/")]
        public ActionResult SubmitRating(string overAllrating, string ratingQuestionAns, string userName, string emailId)
        {
            return View();
        }


        [Route("m/user-reviews/write-review")]
        public ActionResult WriteReview_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }

        [Route("m/user-reviews/review-summary")]
        public ActionResult ReviewSummary_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
        }
    }
}