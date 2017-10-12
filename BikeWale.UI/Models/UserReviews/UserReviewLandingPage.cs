using Bikewale.Common;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Utility;
using System;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewLandingPage
    {
        private readonly IUserReviewsCache _userReviewsCache = null;

        private UserReviewLandingVM objData = null;
        public UserReviewLandingPage(IUserReviewsCache userReviewsCache)
        {
            _userReviewsCache = userReviewsCache;
        }

        public UserReviewLandingVM GetData()
        {
            try
            {
                objData = new UserReviewLandingVM();

                BindWidgets();

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewLandingPage.GetData");
            }
            return objData;
        }

        public void BindWidgets()
        {
            try
            {
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();

                TopRatedBikesWidget objTopRatedBikesWidget = new TopRatedBikesWidget(_userReviewsCache);
                objTopRatedBikesWidget.TopCount = 9;
                objTopRatedBikesWidget.CityId = location.CityId;

                objData.TopRatedBikesWidget = objTopRatedBikesWidget.GetData();


                PopularBikesWithExpertReviewsWidget objBikesWithExpertReviews = new PopularBikesWithExpertReviewsWidget(_userReviewsCache, 9);
                objBikesWithExpertReviews.CityId = location.CityId;
                objData.BikesWithExpertReviews = objBikesWithExpertReviews.GetData();

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewLandingPage.BindWidgets");
            }
        }
    }
}