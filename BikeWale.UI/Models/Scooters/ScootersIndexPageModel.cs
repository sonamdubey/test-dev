using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Videos;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   ScootersIndexPage Model
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance instead of cache for comparison carousel
    /// </summary>
    public class ScootersIndexPageModel
    {
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompare _compareScooters = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Constructor to initialize the member variables
        /// </summary>
        /// <param name="bikeMakes"></param>
        /// <param name="bikeModels"></param>
        /// <param name="newLaunches"></param>
        /// <param name="upcoming"></param>
        /// <param name="compareScooters"></param>
        public ScootersIndexPageModel(IBikeMakesCacheRepository bikeMakes,
            IBikeModels<BikeModelEntity, int> bikeModels,
            INewBikeLaunchesBL newLaunches,
            IUpcoming upcoming,
            IBikeCompare compareScooters,
            ICMSCacheContent articles,
            IVideos videos
            )
        {
            _bikeMakes = bikeMakes;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _upcoming = upcoming;
            _compareScooters = compareScooters;
            _articles = articles;
            _videos = videos;
        }

        public uint CityId { get { return GlobalCityArea.GetGlobalCityArea().CityId; } }
        public ushort BrandTopCount { get; set; }
        public PQSourceEnum PqSource { get; set; }
        public CompareSources CompareSource { get; set; }
        public uint EditorialTopCount { get; set; }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns the Scooters Index Page view model
        /// Modified by : snehal Dange on 28th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        /// <returns></returns>
        public ScootersIndexPageVM GetData()
        {
            ScootersIndexPageVM objVM = null;
            try
            {
                objVM = new ScootersIndexPageVM();

                objVM.City = GlobalCityArea.GetGlobalCityArea();

                BindPageMetas(objVM);
                objVM.Brands = (new BrandWidgetModel(BrandTopCount, _bikeMakes)).GetData(EnumBikeType.Scooters);
                BindPopularBikes(objVM);
                BindNewLaunches(objVM);
                BindUpcoming(objVM);
                BindComparison(objVM);
                BindEditorialWidget(objVM);
                objVM.Page = Entities.Pages.GAPages.Scooters_Landing_Page;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersIndexPageModel.GetData()");
            }
            return objVM;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds Page Metas
        /// Modified by: Dhruv Joshi
        /// Dated: 28th March 2018
        /// Description: Scooters Landing Page Title change
        /// Modified by : Sanskar Gupta on 18 May 2018
        /// Description : Added targeting for city.
        /// </summary>
        /// <param name="objVM"></param>
        private static void BindPageMetas(ScootersIndexPageVM objVM)
        {
            try
            {
                objVM.PageMetaTags.CanonicalUrl = "https://www.bikewale.com/scooters/";
                objVM.PageMetaTags.AlternateUrl = "https://www.bikewale.com/m/scooters/";
                objVM.PageMetaTags.Keywords = "Scooters, Scooty, New scooter, New Scooty, Scooter in India, scooty, Scooter comparison, compare scooter, scooter price, scooty price";
                objVM.PageMetaTags.Description = "Find scooters of Honda, Hero, TVS, Vespa and many more brands. Know about prices, images, colours, specs and reviews of scooters in India";
                objVM.PageMetaTags.Title = "Scooters in India - Scooty Prices, Reviews, Images, Colours - BikeWale";

                GlobalCityAreaEntity city = objVM.City;
                if (city != null && city.CityId > 0)
                {
                    objVM.AdTags.TargetedCity = city.City;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersIndexPageModel.BindPageMetas()");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 2 June 2017
        /// Summary : Bind popular scooter comparisons
        /// </summary>
        private void BindComparison(ScootersIndexPageVM objVM)
        {
            try
            {
                ComparePopularBikes objCompare = new ComparePopularBikes(_compareScooters);
                objCompare.TopCount = 9;
                objCompare.CityId = CityId;
                objCompare.IsScooter = true;
                objVM.ComparePopularScooters = objCompare.GetData();
                objVM.HasComparison = (objVM.ComparePopularScooters != null && objVM.ComparePopularScooters.CompareBikes != null && objVM.ComparePopularScooters.CompareBikes.Any());
                objVM.ComparePopularScooters.CompareSource = CompareSource;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersIndexPageModel.BindComparison");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds upcoming
        /// </summary>
        /// <param name="objVM"></param>
        private void BindUpcoming(ScootersIndexPageVM objVM)
        {
            try
            {
                var upcoming = new UpcomingBikesWidget(_upcoming);
                upcoming.Filters = new UpcomingBikesListInputEntity() { PageSize = 9, PageNo = 1, BodyStyleId = 5 };
                upcoming.SortBy = EnumUpcomingBikesFilter.Default;
                objVM.Upcoming = upcoming.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersIndexPageModel.BindUpcoming()");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds New launches
        /// </summary>
        /// <param name="objVM"></param>
        private void BindNewLaunches(ScootersIndexPageVM objVM)
        {
            try
            {
                var newLaunch = new NewLaunchedWidgetModel(9, _newLaunches);
                newLaunch.BodyStyleId = 5;
                objVM.NewLaunches = newLaunch.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersIndexPageModel.BindNewLaunches()");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds Popular Bikes
        /// </summary>
        /// <param name="objVM"></param>
        private void BindPopularBikes(ScootersIndexPageVM objVM)
        {
            try
            {
                var popular = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, true, false, PqSource, 0);
                popular.CityId = CityId;
                popular.TopCount = 9;
                objVM.PopularBikes = popular.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersIndexPageModel.BindPopularBikes()");
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 14 June 2017
        /// Summary    : Bind Scooter related editorial content
        /// </summary>
        private void BindEditorialWidget(ScootersIndexPageVM objVM)
        {
            try
            {
                RecentNews objNews = new RecentNews(EditorialTopCount, _articles);
                objNews.IsScooter = true;
                objVM.News = objNews.GetData();

                RecentExpertReviews objReviews = new RecentExpertReviews(EditorialTopCount, _articles);
                objReviews.IsScooter = true;
                objVM.ExpertReviews = objReviews.GetData();

                RecentVideos objVideos = new RecentVideos(1, (ushort)EditorialTopCount, _videos);
                objVideos.IsScooter = true;
                objVM.Videos = objVideos.GetData();

                objVM.TabCount = 0;
                objVM.IsNewsActive = false;
                objVM.IsExpertReviewActive = false;
                objVM.IsVideoActive = false;

                if (objVM.News != null && objVM.News.FetchedCount > 0)
                {
                    objVM.TabCount++;
                    objVM.IsNewsActive = true;
                }
                if (objVM.ExpertReviews.FetchedCount > 0)
                {
                    objVM.TabCount++;
                    if (!objVM.IsNewsActive)
                    {
                        objVM.IsExpertReviewActive = true;
                    }
                }
                if (objVM.Videos.FetchedCount > 0)
                {
                    objVM.TabCount++;
                    if (!objVM.IsExpertReviewActive && !objVM.IsNewsActive)
                    {
                        objVM.IsVideoActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ScootersIndexPageModel.BindEditorialWidget()");
            }
        }

    }
}