﻿using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using System;

namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Created by Sajal Gupta on 27-03-2017
    /// This Model will fetch data for service centers details page
    /// </summary>
    public class ServiceCenterDetailsPage
    {
        private uint _makeId, _cityId, _serviceCenterId;

        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IServiceCenter _objSC = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCache;

        public MakeMaskingResponse objResponse;
        public StatusCodes status;

        public uint PopularBikeWidgetTopCount { get; set; }
        public uint UsedBikeWidgetTopCount { get; set; }
        public uint BikeShowroomWidgetTopCount { get; set; }
        public uint OtherServiceCenterWidgetTopCount { get; set; }

        public ServiceCenterDetailsPage(IBikeModels<BikeModelEntity, int> bikeModels, IUsedBikeDetailsCacheRepository objUsedCache, IDealerCacheRepository objDealerCache, IServiceCenter objSC, IBikeMakesCacheRepository bikeMakesCache, string makeMaskingName, uint serviceCenterId)
        {
            _bikeModels = bikeModels;
            _objUsedCache = objUsedCache;
            _objDealerCache = objDealerCache;
            _objSC = objSC;
            _bikeMakesCache = bikeMakesCache;
            _serviceCenterId = serviceCenterId;

            ProcessQuery(makeMaskingName);
        }

        public ServiceCenterDetailsPageVM GetData()
        {
            ServiceCenterDetailsPageVM objVM = null;
            try
            {
                objVM = new ServiceCenterDetailsPageVM();

                objVM.ServiceCenterData = _objSC.GetServiceCenterDataById(_serviceCenterId);

                _cityId = objVM.ServiceCenterData.CityId;

                if (_cityId > 0)
                    objVM.City = new CityHelper().GetCityById(_cityId);

                if (_makeId > 0)
                    objVM.Make = _bikeMakesCache.GetMakeDetails(_makeId);

                MostPopularBikesWidget objPopularBikesWidget = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, false, true, PQSourceEnum.Mobile_ServiceCenter_Listing_CityPage, 0, _makeId);
                objPopularBikesWidget.TopCount = (int)PopularBikeWidgetTopCount;
                objPopularBikesWidget.CityId = _cityId;
                objVM.PopularWidgetData = objPopularBikesWidget.GetData();

                objVM.UsedBikesByMakeList = BindUsedBikeByModel(objVM.City);

                BindDealersWidget(objVM);

                objVM.OtherServiceCentersWidgetData = (new ServiceCentersCard(_objSC, OtherServiceCenterWidgetTopCount, (uint)(objVM.Make.MakeId), objVM.City.CityId, _serviceCenterId)).GetData();

                objVM.BikeScheduleList = _objSC.GetServiceScheduleByMake(_makeId);

                BindPageMetas(objVM);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterDetailsPage.GetData()");
            }
            return objVM;
        }

        private UsedBikeModelsWidgetVM BindUsedBikeByModel(CityEntityBase city)
        {
            UsedBikeModelsWidgetVM UsedBikeModel = new UsedBikeModelsWidgetVM();
            try
            {
                UsedBikeModel.CityDetails = city;
                if (_makeId > 0)
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBikeByModelCountInCity(_makeId, _cityId, UsedBikeWidgetTopCount);
                }
                else
                {
                    UsedBikeModel.UsedBikeModelList = _objUsedCache.GetUsedBikeCountInCity(_cityId, UsedBikeWidgetTopCount);
                }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterDetailsPage.BindUsedBikeByModel()");
            }
            return UsedBikeModel;
        }

        private void BindDealersWidget(ServiceCenterDetailsPageVM objVM)
        {
            try
            {
                DealerCardWidget objDealer = new DealerCardWidget(_objDealerCache, _cityId, _makeId);
                objDealer.TopCount = BikeShowroomWidgetTopCount;
                objVM.DealersWidgetData = objDealer.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ServiceCenterDetailsPage.BindDealersWidget()");
            }
        }

        private void BindPageMetas(ServiceCenterDetailsPageVM objPageVM)
        {

            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.ServiceCenterData != null)
                {
                    objPageVM.PageMetaTags.Title = string.Format("{0} {1} | {0} service center in {1} - BikeWale ", objPageVM.ServiceCenterData.Name, objPageVM.ServiceCenterData.CityName);
                    objPageVM.PageMetaTags.Keywords = string.Format("{0}, {0} {1}, {2} servicing {1}", objPageVM.ServiceCenterData.Name, objPageVM.ServiceCenterData.CityName, objPageVM.Make.MakeName);
                    objPageVM.PageMetaTags.Description = string.Format("{0} is an authorised service center of {1}. Get all details related to servicing cost, pick and drop facility and service schedule from {0}", objPageVM.ServiceCenterData.Name, objPageVM.Make.MakeName);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterIndiaPage.BindPageMetas()");
            }
        }

        private void ProcessQuery(string makeMaskingName)
        {
            objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200)
                {
                    _makeId = objResponse.MakeId;
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