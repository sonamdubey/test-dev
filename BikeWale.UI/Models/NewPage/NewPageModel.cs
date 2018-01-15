using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 31-Mar-2017
    ///  Model for new page
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance instead of cache for comparison carousel    
    /// Modified by : Vivek Singh Tomar on 31st July 2017
    /// Summary : Added IUpcoming for filling upcoming bike list
    /// </summary>
    public class NewPageModel
    {
        #region Variables for dependency injection
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeModelsCacheRepository<int> _cachedModels = null;
        private readonly IBikeCompare _objCompare = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly ICMSCacheContent _expertReviews = null;
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IUpcoming _upcoming = null;
        #endregion

        #region Page level variables
        public ushort TopCount { get; private set; }
        public ushort LaunchedRecordCount { get; private set; }
        public ushort UpcomingRecordCount { get; private set; }
        public bool IsMobile { get; set; }
        public string redirectUrl;
        public StatusCodes status;
        public CompareSources CompareSource { get; set; }

        #endregion

        public NewPageModel(ushort topCount, ushort launchedRcordCount, ushort upcomingRecordCount, IBikeMakesCacheRepository bikeMakes, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> cachedModels, IBikeCompare objCompare, IVideos videos, ICMSCacheContent articles, ICMSCacheContent expertReviews, IUpcoming upcoming, IUserReviewsCache userReviewsCache)
        {
            TopCount = topCount;
            LaunchedRecordCount = launchedRcordCount;
            UpcomingRecordCount = upcomingRecordCount;
            _bikeMakes = bikeMakes;
            _newLaunches = newLaunches;
            _bikeModels = bikeModels;
            _cachedModels = cachedModels;
            _objCompare = objCompare;
            _videos = videos;
            _articles = articles;
            _expertReviews = expertReviews;
            _userReviewsCache = userReviewsCache;
            _upcoming = upcoming;
        }


        /// <summary>
        /// Gets the data for homepage
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// Modified by: Vivek Singh Tomar on 31st July 2017
        /// Summary    : Replaced logic of fetching upcoming bike list.
        /// </returns>
        public NewPageVM GetData()
        {
            NewPageVM objVM = new NewPageVM();
            uint cityId = 0;
            string cityName, cityMaskingName = string.Empty;

            GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
            if (location != null && location.CityId > 0)
            {
                cityId = location.CityId;
                cityName = location.City;
                var cityEntity = new CityHelper().GetCityById(cityId);
                cityMaskingName = cityEntity != null ? cityEntity.CityMaskingName : string.Empty;
                objVM.Location = cityName;
                objVM.LocationMasking = cityMaskingName;
            }
            else
            {
                objVM.Location = "India";
                objVM.LocationMasking = "india";
            }
            BindPageMetas(objVM.PageMetaTags);
            BindAdTags(objVM.AdTags);

            objVM.Brands = new BrandWidgetModel(TopCount, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.New);
            var popularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, true, false);
            popularBikes.TopCount = 9;
            objVM.PopularBikes = popularBikes.GetData();
            objVM.PopularBikes.PageCatId = 5;
            objVM.PopularBikes.PQSourceId = PQSourceEnum.Desktop_HP_MostPopular;


            BikeFilters obj = new BikeFilters();
            obj.CityId = cityId;
            obj.TopCount = 2;
            IEnumerable<MostPopularBikesBase> promotedBikes = _bikeModels.GetAdPromotedBike(obj);
            if (objVM.PopularBikes != null)
                objVM.PopularBikes.Bikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objVM.PopularBikes.Bikes, 9);

            objVM.NewLaunchedBikes = new NewLaunchedWidgetModel(LaunchedRecordCount, _newLaunches).GetData();
            objVM.NewLaunchedBikes.PageCatId = 5;
            objVM.NewLaunchedBikes.PQSourceId = (uint)PQSourceEnum.Desktop_New_NewLaunches;

            objVM.UpcomingBikes = new UpcomingBikesWidgetVM();
            var objFiltersUpcoming = new UpcomingBikesListInputEntity()
            {
                PageSize = UpcomingRecordCount,
                PageNo = 1
            };
            var sortBy = EnumUpcomingBikesFilter.Default;
            objVM.UpcomingBikes.UpcomingBikes = _upcoming.GetModels(objFiltersUpcoming, sortBy);

            BindCompareBikes(objVM, CompareSource, cityId);

            BestBikeByBodyStyle objBestBike = new BestBikeByBodyStyle(_cachedModels);
            objBestBike.topCount = 8;
            objVM.BestBikes = objBestBike.GetData();

            objVM.News = new RecentNews(3, _articles).GetData();

            objVM.Videos = new RecentVideos(1, 3, _videos).GetData();

            objVM.ExpertReviews = new RecentExpertReviews(3, _expertReviews).GetData();

            SetFlags(objVM);

            objVM.RecentUserReviewsList = _userReviewsCache.GetRecentReviews();
            objVM.Source = "New_Bikes_Page";
            objVM.Page = GAPages.New_Bikes_Page;
            return objVM;
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 25 Apr 2017
        /// Summary    : Bind popular comparisons
        /// </summary>
        private void BindCompareBikes(NewPageVM objVM, CompareSources CompareSource, uint cityId)
        {
            ComparePopularBikes objCompare = new ComparePopularBikes(_objCompare);
            objCompare.TopCount = 9;
            objCompare.CityId = cityId;
            objVM.ComparePopularBikes = objCompare.GetData();
            objVM.IsComparePopularBikesAvailable = (objVM.ComparePopularBikes != null && objVM.ComparePopularBikes.CompareBikes != null && objVM.ComparePopularBikes.CompareBikes.Any());
            objVM.ComparePopularBikes.CompareSource = CompareSource;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// Binds the ad tags.
        /// </summary>
        /// <param name="adTags">The ad tags.</param>
        private void SetFlags(NewPageVM Model)
        {
            Model.IsPopularBikesDataAvailable = (Model.PopularBikes != null && Model.PopularBikes.Bikes != null && Model.PopularBikes.Bikes.Any());
            Model.IsNewLaunchedDataAvailable = (Model.NewLaunchedBikes != null && Model.NewLaunchedBikes.Bikes != null && Model.NewLaunchedBikes.Bikes.Any());

            Model.IsUpcomingBikeAvailable = (Model.UpcomingBikes != null && Model.UpcomingBikes.UpcomingBikes != null && Model.UpcomingBikes.UpcomingBikes.Any());

            Model.TabCount = 0;
            Model.IsNewsActive = false;
            Model.IsExpertReviewActive = false;
            Model.IsVideoActive = false;

            if (Model.News.FetchedCount > 0)
            {
                Model.TabCount++;
                Model.IsNewsActive = true;
            }
            if (Model.ExpertReviews.FetchedCount > 0)
            {
                Model.TabCount++;
                if (!Model.IsNewsActive)
                {
                    Model.IsExpertReviewActive = true;
                }
            }
            if (Model.Videos.FetchedCount > 0)
            {
                Model.TabCount++;
                if (!Model.IsExpertReviewActive && !Model.IsNewsActive)
                {
                    Model.IsVideoActive = true;
                }
            }
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// Binds the ad tags.
        /// </summary>
        /// <param name="adTags">The ad tags.</param>
        private void BindAdTags(AdTags adTags)
        {
            adTags.Ad_976x400First = true;
            adTags.Ad_976x400Second = true;
        }

        /// <summary>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// Binds the page metas.
        /// </summary>
        /// <param name="objPage">The object page.</param>
        private void BindPageMetas(PageMetaTags objPage)
        {
            try
            {
                objPage.Title = "New Bikes - Bikes Reviews, Images, Specs, Features, Tips & Advices - BikeWale";
                objPage.Keywords = "new bikes, new bikes prices, new bikes comparisons, bikes dealers, on-road price, bikes research, bikes india, Indian bikes, bike reviews, bike Images, specs, features, tips & advices";
                objPage.Description = "New bikes in India. Search for the right new bikes for you, know accurate on-road price and discounts. Compare new bikes and find dealers.";
                objPage.CanonicalUrl = "https://www.bikewale.com/new-bikes-in-india/";
                objPage.AlternateUrl = "https://www.bikewale.com/m/new-bikes-in-india/";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "HomePageModel.BindMetas()");
            }
        }

    }
}