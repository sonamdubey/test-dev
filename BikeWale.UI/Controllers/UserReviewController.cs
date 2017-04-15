using Bikewale.Models;
using Bikewale.Models.UserReviews;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class UserReviewController : Controller
    {
        // GET: UserReview
        [Route("m/user-reviews/rate-bike/")]
        public ActionResult RateBike_Mobile()
        {
            ModelBase m = new ModelBase();
            return View(m);
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