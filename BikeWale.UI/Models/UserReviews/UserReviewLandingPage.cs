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
                BindPageMetas();
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

                objData.RecentUserReviewsList = new UserReviewSearchWidget(_userReviewsCache).GetData();

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

        public void BindPageMetas()
        {
            try
            {
                if (objData != null && objData.PageMetaTags != null)
                {                   
                    objData.PageMetaTags.Title = "Bike Reviews | Reviews from Owners and Experts- BikeWale";
                    objData.PageMetaTags.Description = "Read reviews about a bike from real owners and experts. Know pros, cons, and tips from real users and experts before buying a bike.";                    
                    objData.PageMetaTags.CanonicalUrl = "https://www.bikewale.com/review/";
                    objData.PageMetaTags.Keywords = "Reviews, Bike reviews, expert review, Bike expert review, Bike user review, owner review, bike owner review, user review, bike user review";
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewLandingPage.BindPageMetas()");
            }
        }
    }
}