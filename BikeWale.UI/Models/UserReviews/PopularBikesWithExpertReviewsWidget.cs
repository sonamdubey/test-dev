using Bikewale.Interfaces.UserReviews;

namespace Bikewale.Models.UserReviews
{
    public class PopularBikesWithExpertReviewsWidget
    {
        private readonly IUserReviewsCache _userReviewsCache = null;

        private readonly ushort _topCount;
        public uint CityId { get; set; }

        public PopularBikesWithExpertReviewsWidget(IUserReviewsCache userReviewsCache, ushort topCount)
        {
            _userReviewsCache = userReviewsCache;
            _topCount = topCount;
        }

        public PopularBikesWithExpertReviewsWidgetVM GetData()
        {
            PopularBikesWithExpertReviewsWidgetVM objData = new PopularBikesWithExpertReviewsWidgetVM();
            if (CityId > 0)
            {
                objData.ExpertReviews = _userReviewsCache.GetPopularBikesWithExpertReviewsByCity(_topCount, CityId);
            }
            else
            {
                objData.ExpertReviews = _userReviewsCache.GetPopularBikesWithExpertReviews(_topCount);
            }

            return objData;
        }
    }
}