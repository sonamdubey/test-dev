using Bikewale.Common;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewLandingPage
    {
        private readonly IUserReviewsCache _userReviewsCache = null;

        private UserReviewLandingVM objUserReviewLanding = null;
        public UserReviewLandingPage(IUserReviewsCache userReviewsCache)
        {
            _userReviewsCache = userReviewsCache;
        }

        public UserReviewLandingVM GetData()
        {            
            try
            {
                objUserReviewLanding = new UserReviewLandingVM();

                BindWidgets();                

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewLandingPage.GetData");
            }
            return objUserReviewLanding;
        }

        public void BindWidgets ()
        {
            try
            {
                GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();

                TopRatedBikesWidget objTopRatedBikesWidget = new TopRatedBikesWidget(_userReviewsCache);
                objTopRatedBikesWidget.TopCount = 9;
                objTopRatedBikesWidget.CityId = location.CityId;

                objUserReviewLanding.TopRatedBikesWidget = objTopRatedBikesWidget.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewLandingPage.BindWidgets");
            }
        }
    }
}