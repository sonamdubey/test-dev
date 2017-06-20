using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.UserReviews;
using BikeWaleOpr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Models.UserReviews
{
    public class ManageUserReviewsRatingsPage
    {
        private readonly IUserReviewsRepository _reviewsRepo = null;        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewsRepo"></param>
        /// <param name="makesRepo"></param>
        public ManageUserReviewsRatingsPage(IUserReviewsRepository reviewsRepo)
        {
            _reviewsRepo = reviewsRepo;            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public ManageUserReviewsPageVM GetData()
        {
            ManageUserReviewsPageVM objPageModel = new ManageUserReviewsPageVM();

            try
            {                
                objPageModel.Reviews = _reviewsRepo.GetRatingsList();

                objPageModel.currentUserId = Convert.ToInt32(CurrentUser.Id);
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "BikewaleOpr.Models.UserReviews.ManageUserReviewsRatingsPage");
            }
            return objPageModel;
        }
    }
}