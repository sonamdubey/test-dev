using Bikewale.Common;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Authors;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Utility;
using System;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewLandingPage
    {
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IAuthors _authors = null;

        private UserReviewLandingVM objData = null;

        public bool IsMobile { get; set; }

        public UserReviewLandingPage(IUserReviewsCache userReviewsCache, ICMSCacheContent articles, IAuthors authors)
        {
            _userReviewsCache = userReviewsCache;
            _articles = articles;
            _authors = authors;
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

                objData.ExpertReviews = new RecentExpertReviews(3, _articles).GetData();

                objData.Authors = _authors.GetAuthorsList(Convert.ToInt32(BWConfiguration.Instance.ApplicationId));

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewLandingPage.BindWidgets");
            }
        }
    }
}