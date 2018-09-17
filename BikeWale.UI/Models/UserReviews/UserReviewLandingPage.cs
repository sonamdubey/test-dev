using Bikewale.Common;
using Bikewale.Entities.Location;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.Authors;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Utility;
using System;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewLandingPage
    {
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IAuthors _authors = null;
        private UserReviewLandingVM objData = null;

        public bool IsMobile { get; set; }
        public ushort BrandsSkipCount { get; set; }

        public UserReviewLandingPage(IUserReviewsCache userReviewsCache, ICMSCacheContent articles, IAuthors authors, IBikeMakesCacheRepository bikeMakes)
        {
            _userReviewsCache = userReviewsCache;
            _bikeMakes = bikeMakes;
            _articles = articles;
            _authors = authors;
        }

        /// Modified by : snehal Dange on 28th Nov 2017
        /// Descritpion : Added ga for page
        public UserReviewLandingVM GetData()
        {
            try
            {
                objData = new UserReviewLandingVM();
                objData.Makes = _bikeMakes.GetMakesByType(Entities.BikeData.EnumBikeType.UserReviews);
                BindWidgets();
                BindPageMetas();
                objData.Page = Entities.Pages.GAPages.Reviews_Landing;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewLandingPage.GetData");
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

                var objExpertReviews = new RecentExpertReviews(9, _articles);
                objExpertReviews.IsViewAllLink = true;
                objData.ExpertReviews = objExpertReviews.GetData();

                objData.Authors = _authors.GetAuthorsList(Convert.ToInt32(BWConfiguration.Instance.ApplicationId));
                objData.Brands = new BrandWidgetModel(BrandsSkipCount, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.UserReviews);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewLandingPage.BindWidgets");
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
                    objData.PageMetaTags.CanonicalUrl = "https://www.bikewale.com/reviews/";
                    objData.PageMetaTags.Keywords = "Reviews, Bike reviews, expert review, Bike expert review, Bike user review, owner review, bike owner review, user review, bike user review";

                    string returnUrl = string.Empty;
                    if (IsMobile)
                    {
                        returnUrl = string.Format("returnUrl=/user-reviews/&sourceid={0}", (int)UserReviewPageSourceEnum.Mobile_UserReview_Landing);
                    }
                    else
                    {
                        returnUrl = string.Format("returnUrl=/user-reviews/&sourceid={0}", (int)UserReviewPageSourceEnum.Desktop_UserReview_Landing);
                    }
                    objData.UserReviewsQueryString = TripleDES.EncryptTripleDES(returnUrl);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewLandingPage.BindPageMetas()");
            }
        }
    }
}