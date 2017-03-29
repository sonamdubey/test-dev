
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Models.Upcoming;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 23 March 2017
    /// Summary :- Model For Dealer India Page
    /// </summary>
    public class DealerShowroomIndiaPage
    {
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly IStateCacheRepository _objStateCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;

        public uint makeId, cityId, topCount;
        public CityEntityBase cityDetails;
        public StatusCodes status;
        public MakeMaskingResponse objResponse;
        public BikeMakeEntityBase objMake;
        private DealerShowroomIndiaPageVM objDealerVM;
        public string redirectUrl;
        //Constructor
        public DealerShowroomIndiaPage(INewBikeLaunchesBL newLaunches, IDealerCacheRepository objDealerCache, IUpcoming upcoming, IUsedBikeDetailsCacheRepository objUsedCache, IStateCacheRepository objStateCache, IBikeMakesCacheRepository<int> bikeMakesCache, string makeMaskingName)
        {
            _objDealerCache = objDealerCache;
            _upcoming = upcoming;
            _objUsedCache = objUsedCache;
            _objStateCache = objStateCache;
            _bikeMakesCache = bikeMakesCache;
            _newLaunches = newLaunches;
            ProcessQuery(makeMaskingName);
        }

        /// <summary>
        /// Created By :- Subodh Jain 23 March 2017
        /// Summary :- Dealer Locator view model data fetching
        /// </summary>
        /// <returns></returns>
        public DealerShowroomIndiaPageVM GetData()
        {
            objDealerVM = new DealerShowroomIndiaPageVM();
            try
            {

                objMake = new MakeHelper().GetMakeNameByMakeId(makeId);
                if (objMake != null)
                    objDealerVM.Make = objMake;

                objDealerVM.States = BindDealersMapCity();
                objDealerVM.DealerCount = objDealerVM.States.TotalDealers;
                objDealerVM.CitiesCount = objDealerVM.States.TotalCities;
                objDealerVM.AllDealers = BindOtherBrandWidget();
                objDealerVM.objUpcomingBikes = BindUpCompingBikesWidget();
                objDealerVM.UsedBikeModel = BindUsedBikeByModel();
                objDealerVM.NewLaunchedBikes = BindNewLaunchesBikes();
                BindPageMetas(objDealerVM.PageMetaTags);


            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.GetData()");
            }
            return objDealerVM;
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
        /// Summary:- Fetching data about dealers of other brands
        /// </summary>
        /// <returns></returns>
        private void BindPageMetas(PageMetaTags objPage)
        {

            try
            {
                objPage.Title = string.Format("{0} Bike Showrooms in India | {0} Bike Dealers in India - BikeWale", objMake.MakeName);
                objPage.Keywords = string.Format("{0} bike dealers, {0} bike showrooms, {0} dealers, {0} showrooms, {0} dealerships, dealerships, test drive, {0} dealer contact number", objMake.MakeName);
                objPage.Description = string.Format("Find the nearest {0} showroom in your city. There are {1} {0} showrooms in {2} cities in India. Get contact details, address, and direction of {0} dealers.", objMake.MakeName, objDealerVM.DealerCount, objDealerVM.CitiesCount);

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindPageMetas()");
            }
        }

        /// <summary>
        /// Created By :- Subodh Jain 25 March 2017
        /// Summary :- State wise dealers count in different cities
        /// </summary>
        /// <returns></returns>
        private DealerLocatorList BindDealersMapCity()
        {
            DealerLocatorList states = new DealerLocatorList();
            try
            {

                states = _objStateCache.GetDealerStatesCities(makeId);
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindDealersMapCity()");
            }
            return states;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Bind data for used bike widget
        /// </summary>
        /// <returns></returns>
        private UsedBikeModelsWidgetVM BindUsedBikeByModel()
        {
            UsedBikeModelsWidgetVM UsedBikeModel = new UsedBikeModelsWidgetVM();
            try
            {
                if (makeId > 0)
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetPopularUsedModelsByMake(makeId, topCount);
                }
                else
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBike(topCount);
                }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindUsedBikeByModel()");
            }
            return UsedBikeModel;

        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Binding data for upcoming bike widget
        /// </summary>
        /// <returns></returns>
        private UpcomingBikesWidgetVM BindUpCompingBikesWidget()
        {
            UpcomingBikesWidgetVM objUpcomingBikes = null;
            try
            {
                UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);

                objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
                {
                    EndIndex = 9,
                    StartIndex = 1
                };
                objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
                objUpcomingBikes = objUpcoming.GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindUpCompingBikesWidget()");
            }
            return objUpcomingBikes;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Binding data for other dealer brands
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DealerBrandEntity> BindOtherBrandWidget()
        {
            IEnumerable<DealerBrandEntity> AllDealerList = null;
            try
            {
                AllDealerList = _objDealerCache.GetDealerByBrandList();
                if (AllDealerList != null && AllDealerList.Count() > 0)
                {
                    AllDealerList = AllDealerList.Where(v => v.MakeId != makeId);
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomIndiaPage.BindOtherBrandWidget()");
            }
            return AllDealerList;
        }

        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Process the input query
        /// </summary>
        /// <returns></returns>
        private void ProcessQuery(string makeMaskingName)
        {
            try
            {
                GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                cityId = currentCityArea.CityId;

                objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);

                if (objResponse != null)
                {
                    if (objResponse.StatusCode == 200)
                    {
                        makeId = objResponse.MakeId;
                        var _cities = _objDealerCache.FetchDealerCitiesByMake(makeId);
                        if (_cities != null && _cities.Count() > 0)
                        {

                            var _city = _cities.FirstOrDefault(x => x.CityId == cityId);
                            if (_city != null)
                            {
                                redirectUrl = String.Format("/{0}-dealer-showrooms-in-{1}/", makeMaskingName, _city.CityMaskingName);
                                status = StatusCodes.RedirectTemporary;
                            }
                        }
                        if (status == 0)
                            status = StatusCodes.ContentFound;
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("DealerShowroomIndiaPage.ProcessQuery() makeMaskingName:{0}", makeMaskingName));
            }
        }

    }
}