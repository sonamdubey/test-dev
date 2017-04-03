using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.HomePage;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.CompareBikes;
using Bikewale.Utility;
using System;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 24-Mar-2017
    /// Summary:  Model for homepage
    /// </summary>
    public class HomePageModel
    {
        #region Variables for dependency injection
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly ICityCacheRepository _IUsedBikesCache = null;
        private readonly IHomePageBannerCacheRepository _cachedBanner = null;
        private readonly IBikeModelsCacheRepository<int> _cachedModels = null;
        private readonly IBikeCompareCacheRepository _cachedCompare = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly ICMSCacheContent _expertReviews = null;
        #endregion

        #region Page level variables
        public ushort TopCount { get; private set; }
        public ushort LaunchedRecordCount { get; private set; }
        public string redirectUrl;


        #endregion

        public HomePageModel(ushort topCount, ushort launchedRcordCount, IBikeMakes<BikeMakeEntity, int> bikeMakes, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, ICityCacheRepository usedBikeCache, IHomePageBannerCacheRepository cachedBanner, IBikeModelsCacheRepository<int> cachedModels, IBikeCompareCacheRepository cachedCompare, IUsedBikeDetailsCacheRepository cachedBikeDetails, IVideos videos, ICMSCacheContent articles, ICMSCacheContent expertReviews)
        {
            TopCount = topCount;
            LaunchedRecordCount = launchedRcordCount;
            _bikeMakes = bikeMakes;
            _newLaunches = newLaunches;
            _bikeModels = bikeModels;
            _IUsedBikesCache = usedBikeCache;
            _cachedBanner = cachedBanner;
            _cachedModels = cachedModels;
            _cachedCompare = cachedCompare;
            _cachedBikeDetails = cachedBikeDetails;
            _videos = videos;
            _articles = articles;
            _expertReviews = expertReviews;
        }


        /// <summary>
        /// Gets the data for homepage
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
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
            objVM.Banner = _cachedBanner.GetHomePageBanner();
            objVM.Brands = new BrandWidgetModel(TopCount, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.New);
            var popularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, true, false);
            popularBikes.TopCount = 9;
            objVM.PopularBikes = popularBikes.GetData();
            objVM.PopularBikes.PageCatId = 5;
            objVM.PopularBikes.PQSourceId = PQSourceEnum.Desktop_HP_MostPopular;

            objVM.NewLaunchedBikes = new NewLaunchedWidgetModel(LaunchedRecordCount, _newLaunches).GetData();
            objVM.NewLaunchedBikes.PageCatId = 5;
            objVM.NewLaunchedBikes.PQSourceId = (uint)PQSourceEnum.Desktop_New_NewLaunches;

            objVM.UpcomingBikes = new UpcomingBikesWidgetVM();
            objVM.UpcomingBikes.UpcomingBikes = _cachedModels.GetUpcomingBikesList(EnumUpcomingBikesFilter.Default, (int)TopCount, null, null, 1);

            objVM.CompareBikes = new ComparisonMinWidget(_cachedCompare, 4, true, EnumBikeType.New).GetData();

            objVM.BestBikes = new BestBikeWidgetModel(null).GetData();

            objVM.UsedBikeCities = new UsedBikeCitiesWidgetModel(cityMaskingName, string.Empty, _IUsedBikesCache).GetData();

            objVM.UsedModels = BindUsedBikeByModel(cityId);

            objVM.News = new RecentNews(3, _articles).GetData();

            objVM.Videos = new RecentVideos(1, 3, _videos).GetData();

            objVM.ExpertReviews = new RecentExpertReviews(3, _expertReviews).GetData();

            SetFlags(objVM);



            return objVM;
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

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "HomePageModel.BindUsedBikeByModel()");
            }

            return UsedBikeModel;

        }

        /// <summary>
        /// Sets the flags.
        /// </summary>
        /// Created by : Sangram Nandkhile on 25-Mar-2017 
        /// <param name="Model">The model.</param>
        private void SetFlags(HomePageVM Model)
        {
            Model.IsPopularBikesDataAvailable = (Model.PopularBikes != null && Model.PopularBikes.Bikes != null && Model.PopularBikes.Bikes.Count() > 0);
            Model.IsNewLaunchedDataAvailable = (Model.NewLaunchedBikes != null && Model.NewLaunchedBikes.Bikes != null && Model.NewLaunchedBikes.Bikes.Count() > 0);
            Model.IsUsedBikeCitiesAvailable = (Model.UsedBikeCities != null && Model.UsedBikeCities.Cities != null && Model.UsedBikeCities.Cities.Count() > 0);
            Model.IsUpcomingBikeAvailable = (Model.UpcomingBikes != null && Model.UpcomingBikes.UpcomingBikes != null && Model.UpcomingBikes.UpcomingBikes.Count() > 0);
            Model.IsUsedModelsAvailable = (Model.UsedModels != null && Model.UsedModels.UsedBikeModelList != null && Model.UsedModels.UsedBikeModelList.Count() > 0);
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

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "HomePageModel.BindMetas()");
            }
        }

    }
}