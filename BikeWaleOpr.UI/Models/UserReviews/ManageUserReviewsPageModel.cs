using BikewaleOpr.Entity.UserReviews;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.UserReviews;
using BikeWaleOpr.Common;
using System;

namespace BikewaleOpr.Models.UserReviews
{
    /// <summary>
    /// 
    /// </summary>
    public class ManageUserReviewsPageModel
    {
        private readonly IUserReviewsRepository _reviewsRepo = null;
        private readonly IBikeMakes _makesRepo = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reviewsRepo"></param>
        /// <param name="makesRepo"></param>
        public ManageUserReviewsPageModel(IUserReviewsRepository reviewsRepo, IBikeMakes makesRepo)
        {
            _reviewsRepo = reviewsRepo;
            _makesRepo = makesRepo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public ManageUserReviewsPageVM GetData(ReviewsInputFilters filters)
        {
            ManageUserReviewsPageVM objPageModel = new ManageUserReviewsPageVM();

            try
            {
                if ((ushort)filters.ReviewStatus == 0)
                    filters.ReviewStatus = ReviewsStatus.Pending;

                objPageModel.selectedFilters = filters;

                objPageModel.Makes = _makesRepo.GetMakes(8);

                objPageModel.Reviews = _reviewsRepo.GetReviewsList(filters);

                objPageModel.currentUserId = Convert.ToInt32(CurrentUser.Id);
            }
            catch (Exception ex)
            {
                ErrorClass err = new ErrorClass(ex, "BikewaleOpr.Models.UserReviews.ManageUserReviewsPageModel");
            }
            return objPageModel;
        }

    }   // Class
}   // namespace