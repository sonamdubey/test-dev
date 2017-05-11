using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.UserReviews;
using BikewaleOpr.Models.UserReviews;

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
        private readonly IBikeMakes _makesRepo = null;

        /// <summary>
        /// Constructor to initialize all the dependencies
        /// </summary>
        /// <param name="reviewsRepo"></param>
        /// <param name="makesRepo"></param>
        public UserReviewsController(IUserReviewsRepository reviewsRepo, IBikeMakes makesRepo)
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

    }   // Class
}   // namespace