
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
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

        public uint makeId, cityId, topCount;
        public CityEntityBase cityDetails;
        public StatusCodes status;
        public MakeMaskingResponse objResponse;
        public BikeMakeEntityBase objMake;
        private DealerShowroomIndiaPageVM objDealerVM;
        //Constructor
        public DealerShowroomIndiaPage(IDealerCacheRepository objDealerCache, IUpcoming upcoming, IUsedBikeDetailsCacheRepository objUsedCache, IStateCacheRepository objStateCache, IBikeMakesCacheRepository<int> bikeMakesCache, string makeMaskingName)
        {
            _objDealerCache = objDealerCache;
            _upcoming = upcoming;
            _objUsedCache = objUsedCache;
            _objStateCache = objStateCache;
            _bikeMakesCache = bikeMakesCache;
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
        private UsedBikeModels BindUsedBikeByModel()
        {
            UsedBikeModels UsedBikeModel = new UsedBikeModels();
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
            objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200)
                {
                    makeId = objResponse.MakeId;
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

    }
}