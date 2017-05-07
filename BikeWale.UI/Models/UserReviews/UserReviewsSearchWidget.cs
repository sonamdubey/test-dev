using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.UserReviews;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewsSearchWidget
    {
        private InputFilters _filters = null;
        private readonly IUserReviewsCache _userReviewsCacheRepo = null;
        public UserReviewsSearchWidget(InputFilters filters, IUserReviewsCache userReviewsCacheRepo)
        {
            _filters = filters;
            _userReviewsCacheRepo = userReviewsCacheRepo;
        }

        public UserReviewsSearchVM GetData()
        {
            UserReviewsSearchVM objData = new UserReviewsSearchVM();
            objData.UserReviews = _userReviewsCacheRepo.GetUserReviewsList(_filters);

            if (objData.ReviewsInfo.Make == null || objData.ReviewsInfo.Model == null)
            {
                objData.ReviewsInfo = _userReviewsCacheRepo.GetBikeReviewsInfo(objData.ModelId);
            }

            if (objData.ReviewsInfo != null)
            {
                //set bike data and other properties
            }

            return objData;
        }
    }
}