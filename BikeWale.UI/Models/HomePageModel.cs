using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;

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
        #endregion

        #region Page level variables
        public ushort TopCount { get; private set; }
        public ushort LaunchedRecordCount { get; private set; }
        public string redirectUrl;
        public StatusCodes status;
        #endregion

        public HomePageModel(ushort topCount, ushort launchedRcordCount, IBikeMakes<BikeMakeEntity, int> bikeMakes, INewBikeLaunchesBL newLaunches)
        {
            TopCount = topCount;
            LaunchedRecordCount = launchedRcordCount;
            _bikeMakes = bikeMakes;
            _newLaunches = newLaunches;
        }

        /// <summary>
        /// Gets the data for homepage controller
        /// </summary>
        /// <returns>
        /// Created by : Sangram Nandkhile on 24-Mar-2017 
        /// </returns>
        public HomePageVM GetData()
        {
            HomePageVM objVM = new HomePageVM();
            objVM.Brands = new BrandWidgetModel(TopCount, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.New);
            objVM.NewLaunchedBikes = new NewLaunchedWidgetModel(LaunchedRecordCount, _newLaunches).GetData();
            objVM.NewLaunchedBikes.PageCatId = 5;
            objVM.NewLaunchedBikes.PQSourceId = (uint)PQSourceEnum.Desktop_New_NewLaunches;
            objVM.BestBikes = new BestBikeWidgetModel(null).GetData();
            return objVM;
        }

    }
}