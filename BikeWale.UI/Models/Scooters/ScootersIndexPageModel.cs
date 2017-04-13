using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Compare;
using Bikewale.Models;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   ScootersIndexPage Model
    /// </summary>
    public class ScootersIndexPageModel
    {
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompareCacheRepository _compareScooters = null;

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Constructor to initialize the member variables
        /// </summary>
        /// <param name="bikeMakes"></param>
        /// <param name="bikeModels"></param>
        /// <param name="newLaunches"></param>
        /// <param name="upcoming"></param>
        /// <param name="compareScooters"></param>
        public ScootersIndexPageModel(IBikeMakes<BikeMakeEntity, int> bikeMakes,
            IBikeModels<BikeModelEntity, int> bikeModels,
            INewBikeLaunchesBL newLaunches,
            IUpcoming upcoming,
            IBikeCompareCacheRepository compareScooters
            )
        {
            _bikeMakes = bikeMakes;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _upcoming = upcoming;
            _compareScooters = compareScooters;
        }

        public uint CityId { get { return GlobalCityArea.GetGlobalCityArea().CityId; } }
        public ushort BrandTopCount { get; set; }
        public PQSourceEnum PqSource { get; set; }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Returns the Scooters Index Page view model
        /// </summary>
        /// <returns></returns>
        public ScootersIndexPageVM GetData()
        {
            ScootersIndexPageVM objVM = null;
            try
            {
                objVM = new ScootersIndexPageVM();
                BindPageMetas(objVM);
                objVM.Brands = (new BrandWidgetModel(BrandTopCount, _bikeMakes)).GetData(EnumBikeType.Scooters);
                BindPopularBikes(objVM);
                BindNewLaunches(objVM);
                BindUpcoming(objVM);
                BindComparison(objVM);
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "ScootersIndexPageModel.GetData()");
            }
            return objVM;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds Page Metas
        /// </summary>
        /// <param name="objVM"></param>
        private static void BindPageMetas(ScootersIndexPageVM objVM)
        {
            try
            {
                objVM.PageMetaTags.CanonicalUrl = "https://www.bikewale.com/new-scooters/";
                objVM.PageMetaTags.AlternateUrl = "https://www.bikewale.com/m/new-scooters/";
                objVM.PageMetaTags.Keywords = "Scooters, Scooty, New scooter, New Scooty, Scooter in India, scooty, Scooter comparison, compare scooter, scooter price, scooty price";
                objVM.PageMetaTags.Description = "Find scooters of Honda, Hero, TVS, Vespa and many more brands. Know about prices, images, colours, specs and reviews of scooters in India";
                objVM.PageMetaTags.Title = "New Scooters - Scooters Prices, Reviews, Images, Colours - BikeWale";
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "ScootersIndexPageModel.BindPageMetas()");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Mar 2017
        /// Description :   Binds Comparison
        /// </summary>
        /// <param name="objVM"></param>
        private void BindComparison(ScootersIndexPageVM objVM)
        {
            try
            {
                var compare = new CompareBikes.ComparisonMinWidget(_compareScooters, 4, true, EnumBikeType.Scooters);
                objVM.Comparison = compare.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass er = new ErrorClass(ex, "ScootersIndexPageModel.BindComparison()");
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
                ErrorClass er = new ErrorClass(ex, "ScootersIndexPageModel.BindUpcoming()");
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
                ErrorClass er = new ErrorClass(ex, "ScootersIndexPageModel.BindNewLaunches()");
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
                ErrorClass er = new ErrorClass(ex, "ScootersIndexPageModel.BindPopularBikes()");
            }
        }

    }
}