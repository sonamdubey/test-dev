using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.HomePage;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.Videos;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 24-Mar-2017
    /// Summary:  Model for homepage
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance instead of cache for comaprison carousel
    /// Modified by : Vivek Singh Tomar on 31st July 2017
    /// Summary : Added IUpcoming for filling upcoming bike list
    /// </summary>
    public class HomePageModel
    {
        #region Variables for dependency injection
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly ICityCacheRepository _IUsedBikesCache = null;
        private readonly IHomePageBannerCacheRepository _cachedBanner = null;
        private readonly IBikeModelsCacheRepository<int> _cachedModels = null;
        private readonly IBikeCompare _objCompare = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IAdSlot _adSlot = null;
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        #endregion

        #region Page level variables
        public ushort TopCount { get; private set; }
        public ushort LaunchedRecordCount { get; private set; }
        public ushort UpcomingRecordCount { get; private set; }
        public bool IsMobile { get; set; }
        public CompareSources CompareSource { get; set; }


        #endregion

        /// <summary>
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added IAdSlot.
        /// </summary>
        public HomePageModel(ushort topCount, ushort launchedRcordCount, ushort upcomingRecordCount, IBikeMakesCacheRepository bikeMakes, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, ICityCacheRepository usedBikeCache, IHomePageBannerCacheRepository cachedBanner, IBikeModelsCacheRepository<int> cachedModels, IBikeCompare objCompare, IUsedBikeDetailsCacheRepository cachedBikeDetails, IVideos videos, ICMSCacheContent articles, IUpcoming upcoming, IUserReviewsCache userReviewsCache, IAdSlot adSlot)
        {
            TopCount = topCount;
            LaunchedRecordCount = launchedRcordCount;
            UpcomingRecordCount = upcomingRecordCount;
            _bikeMakes = bikeMakes;
            _newLaunches = newLaunches;
            _bikeModels = bikeModels;
            _IUsedBikesCache = usedBikeCache;
            _cachedBanner = cachedBanner;
            _cachedModels = cachedModels;
            _objCompare = objCompare;
            _cachedBikeDetails = cachedBikeDetails;
            _videos = videos;
            _articles = articles;
            _userReviewsCache = userReviewsCache;
            _upcoming = upcoming;
            _adSlot = adSlot;
        }


        /// <summary>
        /// Gets the data for homepage
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// Modified by : Aditi Srivastava on 25 Apr 2017
        /// Summary  :  Added different functions to bind popular comparison carousel for msite and desktop
        /// Modified by : Aditi Srivastava on 3 June 2017
        /// Summary     : Added single function for comaprison carousel for both msite and desktop
        /// Modified by: Vivek Singh Tomar on 31 July 2017
        /// Summary    : Replaced logic of fetching upcoming bike list.
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added call to BindAdSlotTags.
        /// Modified By: Snehal Dange on 24th Nov 2017
        /// Description: Changed popularBikes.TopCount from 9 to 8
        /// 
        /// </returns>
        public HomePageVM GetData()
        {
            HomePageVM objVM = new HomePageVM();

            #region Variable initialization

            uint cityId = 0;
            string cityName, cityMaskingName = string.Empty;
            CityEntityBase cityBase = null;
            GlobalCityAreaEntity location = GlobalCityArea.GetGlobalCityArea();
            if (location != null && location.CityId > 0)
            {
                cityId = location.CityId;
                cityName = location.City;
                var cityEntity = new CityHelper().GetCityById(cityId);
                cityMaskingName = cityEntity != null ? cityEntity.CityMaskingName : string.Empty;
                objVM.Location = cityName;
                objVM.LocationMasking = cityMaskingName;
                cityBase = new CityEntityBase()
                {
                    CityId = cityId,
                    CityMaskingName = cityMaskingName,
                    CityName = cityName
                };
            }
            else
            {
                objVM.Location = "India";
                objVM.LocationMasking = "india";
            }
            #endregion

            BindPageMetas(objVM.PageMetaTags);
            BindAdTags(objVM.AdTags);
            objVM.Banner = _cachedBanner.GetHomePageBanner(IsMobile ? (uint)2 : 1);
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
                objVM.PopularBikes.Bikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objVM.PopularBikes.Bikes);

            objVM.NewLaunchedBikes = new NewLaunchedWidgetModel(LaunchedRecordCount, _newLaunches).GetData();
            objVM.NewLaunchedBikes.PageCatId = 1;
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

            string cityWidgetTitle = string.Empty, cityWidgetHref = string.Empty;
            cityWidgetTitle = "Second hand bikes in India";
            cityWidgetHref = "/used/bikes-in-india/";
            objVM.UsedBikeCities = new UsedBikeCitiesWidgetModel(cityWidgetTitle, cityWidgetHref, _IUsedBikesCache).GetData();

            objVM.UsedModels = BindUsedBikeByModel(cityId);

            objVM.News = new RecentNews(3, _articles).GetData();

            objVM.Videos = new RecentVideos(1, 3, _videos).GetData();

            objVM.ExpertReviews = new RecentExpertReviews(3, _articles).GetData();
            objVM.Page = GAPages.HP;
            SetFlags(objVM);

            objVM.RecentUserReviewsList = new UserReviewSearchWidget(_userReviewsCache).GetData();
            objVM.Source = "HP";
            BindAdSlotTags(objVM);
            return objVM;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Bind ad slot to adtags.
        /// </summary>
        /// <param name="objVM"></param>
        private void BindAdSlotTags(HomePageVM objVM)
        {
            try
            {
                if (objVM.AdTags != null)
                {
                    objVM.AdTags.Ad_292x360 = _adSlot.CheckAdSlotStatus("Ad_292x360"); //For best bikes widget scooters desktop
                    objVM.AdTags.Ad_200x211 = _adSlot.CheckAdSlotStatus("Ad_200x211");  //Forbest bikes widget scooters mobile
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "ModelPage.BindAdSlotTags");
            }
        }
        private UsedBikeModelsWidgetVM BindUsedBikeByModel(uint cityId)
        {
            UsedBikeModelsWidgetVM UsedBikeModel = new UsedBikeModelsWidgetVM();
            try
            {

                UsedBikeModelsWidgetModel objUsedBike = new UsedBikeModelsWidgetModel(9, _cachedBikeDetails);

                if (cityId > 0)
                    objUsedBike.cityId = cityId;
                UsedBikeModel = objUsedBike.GetData();
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, "HomePageModel.BindUsedBikeByModel()");
            }

            return UsedBikeModel;

        }

        /// <summary>
        /// Created by : Aditi Srivastava on 25 Apr 2017
        /// Summary    : Bind popular comparisons
        /// </summary>
        private void BindCompareBikes(HomePageVM objVM, CompareSources CompareSource, uint cityId)
        {
            ComparePopularBikes objCompare = new ComparePopularBikes(_objCompare);
            objCompare.TopCount = 9;
            objCompare.CityId = cityId;
            objVM.ComparePopularBikes = objCompare.GetData();
            objVM.IsComparePopularBikesAvailable = (objVM.ComparePopularBikes != null && objVM.ComparePopularBikes.CompareBikes != null && objVM.ComparePopularBikes.CompareBikes.Any());
            objVM.ComparePopularBikes.CompareSource = CompareSource;
        }

        /// <summary>
        /// Sets the flags.
        /// </summary>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// <param name="Model">The model.</param>
        private void SetFlags(HomePageVM Model)
        {
            Model.IsPopularBikesDataAvailable = (Model.PopularBikes != null && Model.PopularBikes.Bikes != null && Model.PopularBikes.Bikes.Any());
            Model.IsNewLaunchedDataAvailable = (Model.NewLaunchedBikes != null && Model.NewLaunchedBikes.Bikes != null && Model.NewLaunchedBikes.Bikes.Any());
            Model.IsUsedBikeCitiesAvailable = (Model.UsedBikeCities != null && Model.UsedBikeCities.Cities != null && Model.UsedBikeCities.Cities.Any());
            Model.IsUpcomingBikeAvailable = (Model.UpcomingBikes != null && Model.UpcomingBikes.UpcomingBikes != null && Model.UpcomingBikes.UpcomingBikes.Any());
            Model.IsUsedModelsAvailable = (Model.UsedModels != null && Model.UsedModels.UsedBikeModelList != null && Model.UsedModels.UsedBikeModelList.Any());
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
                objPage.Title = "New Bikes, Used Bikes, Bike Prices, Reviews & Images in India";
                objPage.Keywords = "new bikes, used bikes, buy used bikes, sell your bike, bikes prices, reviews, Images, news, compare bikes, Instant Bike On-Road Price";
                objPage.Description = "BikeWale - India's favourite bike portal. Find new and used bikes, buy or sell your bikes, compare new bikes prices & values.";
                objPage.CanonicalUrl = "https://www.bikewale.com/";
                objPage.AlternateUrl = "https://www.bikewale.com/m/";

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "HomePageModel.BindMetas()");
            }
        }

    }
}