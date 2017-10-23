using Bikewale.Common;
using Bikewale.Interfaces.UserReviews;
using System;

namespace Bikewale.Models.UserReviews
{
    public class TopRatedBikesWidget
    {
        private readonly IUserReviewsCache _userReviewsCache = null;

        public uint TopCount { get; set; }
        public uint CityId { get; set; }

        public TopRatedBikesWidget(IUserReviewsCache userReviewsCache)
        {
            _userReviewsCache = userReviewsCache;
        }

        public TopRatedBikesWidgetVM GetData()
        {
            TopRatedBikesWidgetVM objTopRatedBikesWidget = null;
            try
            {
                objTopRatedBikesWidget = new TopRatedBikesWidgetVM();

                if (CityId > 0)
                    objTopRatedBikesWidget.Bikes = _userReviewsCache.GetTopRatedBikes(TopCount, CityId);
                else
                    objTopRatedBikesWidget.Bikes = _userReviewsCache.GetTopRatedBikes(TopCount);

                objTopRatedBikesWidget.WidgetHeading = "Top rated bikes";
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("TopRatedBikesWidget.GetData: topCount: {0}, CityId {1}", TopCount, CityId));
            }
            return objTopRatedBikesWidget;
        }
    }
}