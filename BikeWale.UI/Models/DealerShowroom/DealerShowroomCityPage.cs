﻿
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Used;
using Bikewale.Memcache;
using Bikewale.Models.Make;
using System;
using System.Linq;
namespace Bikewale.Models.DealerShowroom
{
    /// <summary>
    /// Created By :- Subodh Jain 27 March 2017
    /// Summary :- Sealer Showroom in city page model
    /// </summary>
    public class DealerShowroomCityPage
    {
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;

        public MakeMaskingResponse objResponse;
        public uint cityId, makeId, topCount;
        public StatusCodes status;
        public BikeMakeEntityBase objMake;
        public CityEntityBase CityDetails;
        //Constructor
        public DealerShowroomCityPage(IDealerCacheRepository objDealerCache, IUsedBikeDetailsCacheRepository objUsedCache, IBikeMakesCacheRepository<int> bikeMakesCache, string makeMaskingName, string cityMaskingName)
        {
            _objDealerCache = objDealerCache;
            _bikeMakesCache = bikeMakesCache;
            _objUsedCache = objUsedCache;
            ProcessQuery(makeMaskingName, cityMaskingName);
        }
        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Fetch Data realted to Dealer in city Page
        /// </summary>
        /// <returns></returns>
        public DealerShowroomCityPageVM GetData()
        {
            DealerShowroomCityPageVM objDealerVM = new DealerShowroomCityPageVM();

            try
            {
                objMake = new MakeHelper().GetMakeNameByMakeId(makeId);
                if (objMake != null)
                    objDealerVM.Make = objMake;
                if (cityId > 0)
                {
                    CityDetails = new CityHelper().GetCityById(cityId);
                    objDealerVM.CityDetails = CityDetails;
                }
                objDealerVM.DealersList = BindDataDealers();
                if (objDealerVM.DealersList != null && objDealerVM.DealersList.Dealers != null)
                {
                    objDealerVM.TotalDealers = (uint)objDealerVM.DealersList.Dealers.Count();
                }
                objDealerVM.DealerCountCity = BindOtherDealerInCitiesWidget();
                objDealerVM.UsedBikeModel = BindUsedBikeByModel();
                objDealerVM.BrandCityPopupWidget = new BrandCityPopupModel(EnumBikeType.Dealer, (uint)objMake.MakeId, cityId).GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomCityPage.GetData()");
            }
            return objDealerVM;
        }

        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Fetch Data realted to Dealer in city Page
        /// </summary>
        /// <returns></returns>
        private DealersEntity BindDataDealers()
        {
            DealersEntity objDealerList = null;
            try
            {
                objDealerList = _objDealerCache.GetDealerByMakeCity(cityId, makeId);
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomCityPage.BindDataDealers()");
            }
            return objDealerList;
        }
        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Fetch Data For other dealers in near by cities
        /// </summary>
        /// <returns></returns>
        private NearByCityDealer BindOtherDealerInCitiesWidget()
        {
            NearByCityDealer objDealer = new NearByCityDealer();
            try
            {
                objDealer.objDealerInNearCityList = _objDealerCache.FetchNearByCityDealersCount(makeId, cityId);
                if (objDealer != null && objDealer.objDealerInNearCityList != null && objDealer.objDealerInNearCityList.Count() > 0)
                {
                    objDealer.objDealerInNearCityList = objDealer.objDealerInNearCityList.Take((int)topCount);
                }
                objDealer.Make = objMake;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomCityPage.BindOtherDealerInCitiesWidget()");

            }

            return objDealer;
        }
        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To process in put query string
        /// </summary>
        /// <returns></returns>
        private void ProcessQuery(string makeMaskingName, string cityMaskingName)
        {
            objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200)
                {
                    makeId = objResponse.MakeId;
                    cityId = CitiMapping.GetCityId(cityMaskingName);
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
        /// <summary>
        /// Created By :- Subodh Jain 27 March 2017
        /// Summary :- To Fetch Data realted to Used Bike in city
        /// </summary>
        /// <returns></returns>
        private UsedBikeModels BindUsedBikeByModel()
        {
            UsedBikeModels UsedBikeModel = new UsedBikeModels();
            try
            {
                if (makeId > 0)
                {
                    if (cityId > 0)
                        UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBikeByModelCountInCity(makeId, cityId, topCount);
                    else
                        UsedBikeModel.UsedBikeModelList = _objUsedCache.GetPopularUsedModelsByMake(makeId, topCount);
                }
                else
                {
                    if (cityId > 0)
                        UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBikeCountInCity(cityId, topCount);
                    else
                        UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBike(topCount);

                }
                if (cityId > 0)
                    UsedBikeModel.CityDetails = CityDetails;
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "DealerShowroomCityPage.BindUsedBikeByModel()");
            }

            return UsedBikeModel;

        }
    }
}