using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.Location;
using Bikewale.Models;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 24-Mar-2017
    /// Summary:  Model for homepage
    /// </summary>
    public class HomePageModel
    {
        #region Variables for dependency injection
        private readonly uint _cityId;
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly ICityCacheRepository _IUsedBikesCache = null;
        #endregion

        #region Page level variables
        public ushort TopCount { get; private set; }
        public ushort LaunchedRecordCount { get; private set; }
        public string redirectUrl;
        public StatusCodes status;
        #endregion

        public HomePageModel(uint cityId, ushort topCount, ushort launchedRcordCount, IBikeMakes<BikeMakeEntity, int> bikeMakes, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, ICityCacheRepository usedBikeCache)
        {
            _cityId = cityId;
            TopCount = topCount;
            LaunchedRecordCount = launchedRcordCount;
            _bikeMakes = bikeMakes;
            _newLaunches = newLaunches;
            _bikeModels = bikeModels;
            _IUsedBikesCache = usedBikeCache;
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
            objVM.Brands = new BrandWidgetModel(TopCount, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.New);

            var popularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, true);
            popularBikes.TopCount = 9;
            objVM.PopularBikes = popularBikes.GetData();
            objVM.PopularBikes.PageCatId = 5;
            objVM.PopularBikes.PQSourceId = PQSourceEnum.Desktop_HP_MostPopular;

            objVM.NewLaunchedBikes = new NewLaunchedWidgetModel(LaunchedRecordCount, _newLaunches).GetData();
            objVM.NewLaunchedBikes.PageCatId = 5;
            objVM.NewLaunchedBikes.PQSourceId = (uint)PQSourceEnum.Desktop_New_NewLaunches;

            objVM.BestBikes = new BestBikeWidgetModel(null).GetData();

            string cityMasking = string.Empty;
            if (_cityId > 0)
            {
                cityMasking = string.Empty;
            }
            objVM.UsedBikeCities = new UsedBikeCitiesWidgetModel(cityMasking, string.Empty, _IUsedBikesCache).GetData();
            return objVM;
        }

    }
}