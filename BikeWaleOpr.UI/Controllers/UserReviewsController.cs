using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.UserReviews;
using BikewaleOpr.Models.UserReviews;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created by : Ashish G. Kamble on 15 Apr 2017
    /// Summary : Controller have all methods to manage the user reviews
    /// </summary>
    [Authorize]
    public class UserReviewsController : Controller
    {
        private readonly IUserReviewsRepository _reviewsRepo = null;
        private readonly IBikeMakesRepository _makesRepo = null;

        /// <summary>
        /// Constructor to initialize all the dependencies
        /// </summary>
        /// <param name="reviewsRepo"></param>
        /// <param name="makesRepo"></param>
        public UserReviewsController(IUserReviewsRepository reviewsRepo, IBikeMakesRepository makesRepo)
        {
            _reviewsRepo = reviewsRepo;
            _makesRepo = makesRepo;            
        }

        // GET: UserReviews
        /// <summary>
        /// Function to manage the user reviews (user reviews list page)
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(ReviewsInputFilters filters)
        {
            ManageUserReviewsPageModel objPageModel = new ManageUserReviewsPageModel(_reviewsRepo, _makesRepo);

            ManageUserReviewsPageVM pageVM = objPageModel.GetData(filters);

            return View(pageVM);

        }   // End of Index method

        /// <summary>
        /// Created by Sajal Gupta on 19-06-2017
        /// Descrioption : This method will fetch manage review ratings page.
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageRatings()
        {
            ManageUserReviewsRatingsPage objPageModel = new ManageUserReviewsRatingsPage(_reviewsRepo);

            ManageUserReviewsPageVM pageVM = objPageModel.GetData();

            return View(pageVM);
        }


    }   // Class
}   // namespace