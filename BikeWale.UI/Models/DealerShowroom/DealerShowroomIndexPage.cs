
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Dealer;
using System;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class DealerShowroomIndexPage
    {


        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;

        protected string makeMaskingName = string.Empty, redirectUrl = string.Empty;
        public uint MakeId;
        public ushort TopCount;
        public StatusCodes status;
        public DealerShowroomIndexPage(IBikeMakes<BikeMakeEntity, int> bikeMakes, IDealerCacheRepository objDealerCache, IBikeMakesCacheRepository<int> objMakeCache, IUpcoming upcoming, INewBikeLaunchesBL newLaunches, ushort topCount)
        {
            _newLaunches = newLaunches;
            _objDealerCache = objDealerCache;
            _upcoming = upcoming;
            _objMakeCache = objMakeCache;
            _bikeMakes = bikeMakes;
            TopCount = topCount;
        }
        /// <summary>
        /// Created By :- Subodh Jain 23 March 2017
        /// Summary :- Dealer Locator view model data fetching
        /// </summary>
        /// <returns></returns>
        public IndexVM GetData()
        {
            IndexVM objDealerVM = new IndexVM();
            try
            {

                objDealerVM.MakesList = BindMakeList();
                objDealerVM.objUpcomingBikes = BindUpCompingBikesWidget();
                objDealerVM.BestBikes = new BestBikeWidgetModel(null).GetData();
                objDealerVM.NewLaunchedBikes = BindNewLaunchesBikes();
                objDealerVM.Brands = new BrandWidgetModel(TopCount, _bikeMakes).GetData(Entities.BikeData.EnumBikeType.Dealer);
                BindPageMetas(objDealerVM.PageMetaTags);


            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndexPage.GetData()");
            }
            return objDealerVM;
        }


        /// <summary>
        /// Created By :- Subodh Jain 29 March 2017
        /// Summary :- Bind MakeList
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BikeMakeEntityBase> BindMakeList()
        {
            IEnumerable<BikeMakeEntityBase> _makes;
            _makes = _objMakeCache.GetMakesByType(EnumBikeType.Dealer);
            return _makes;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about dealers of other brands
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(PageMetaTags objPage)
        {

            try
            {
                objPage.Title = string.Format("New Bike Showroom in India | Find Authorized Bike Dealers - BikeWale");
                objPage.Keywords = string.Format("new bike dealers, new bike showrooms, bike dealers, bike showrooms, showrooms, dealerships");
                objPage.Description = string.Format("Locate new bike showrooms and authorized bike dealers in India. Find new bike dealer information for more than 200 cities in India.");

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindPageMetas()");
            }
        }
        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Fetching data about New Bike Launches
        /// </summary>
        /// <returns></returns>
        private NewLaunchedWidgetVM BindNewLaunchesBikes()
        {
            NewLaunchedWidgetVM NewLaunchedbikes = null;
            try
            {
                NewLaunchedWidgetModel objNewLaunched = new NewLaunchedWidgetModel(9, _newLaunches);
                NewLaunchedbikes = objNewLaunched.GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindNewLaunchesBikes()");
            }
            return NewLaunchedbikes;

        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Binding data for upcoming bike widget
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UpcomingBikeEntity> BindUpCompingBikesWidget()
        {
            IEnumerable<UpcomingBikeEntity> objUpcomingBikes = null;
            try
            {
                var objFiltersUpcoming = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
                {
                    EndIndex = 9,
                    StartIndex = 1
                };
                var sortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
                objUpcomingBikes = _upcoming.GetModels(objFiltersUpcoming, sortBy);
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindUpCompingBikesWidget()");
            }
            return objUpcomingBikes;
        }
    }
}