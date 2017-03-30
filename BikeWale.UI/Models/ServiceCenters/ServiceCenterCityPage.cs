using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Memcache;
using Bikewale.Models.Make;
using System;
using System.Linq;

namespace Bikewale.Models.ServiceCenters
{
    public class ServiceCenterCityPage
    {
        private string _cityMaskingName;
        private string _makeMaskingName;
        private uint _makeId, _cityId;

        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache;
        private readonly IServiceCenter _objSC;
        private readonly IServiceCenterCacheRepository _objSCCache;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;

        public MakeMaskingResponse objResponse;
        public StatusCodes status;

        public uint NearByCitiesWidgetTopCount { get; set; }
        public uint UsedBikeWidgetTopCount { get; set; }
        public uint BikeShowroomWidgetTopCount { get; set; }

        public ServiceCenterCityPage(IDealerCacheRepository objDealerCache, IUsedBikeDetailsCacheRepository objUsedCache, IBikeModels<BikeModelEntity, int> bikeModels, IServiceCenterCacheRepository objSCCache, IServiceCenter objSC, IBikeMakesCacheRepository<int> bikeMakesCache, string cityMaskingName, string makeMaskingName)
        {
            _objSC = objSC;
            _objSCCache = objSCCache;
            _bikeModels = bikeModels;
            _objDealerCache = objDealerCache;
            _objUsedCache = objUsedCache;
            _bikeMakesCache = bikeMakesCache;
            _cityMaskingName = cityMaskingName;
            _makeMaskingName = makeMaskingName;

            ProcessQuery(makeMaskingName, cityMaskingName);
        }

        public ServiceCenterCityPageVM GetData()
        {
            ServiceCenterCityPageVM objVM = null;
            try
            {
                objVM = new ServiceCenterCityPageVM();

                if (_cityId > 0)
                    objVM.City = new CityHelper().GetCityById(_cityId);

                if (_makeId > 0)
                    objVM.Make = new MakeHelper().GetMakeNameByMakeId(_makeId);

                objVM.ServiceCentersListObject = _objSC.GetServiceCentersByCity(_cityId, (int)_makeId);
                objVM.NearByCityWidgetData = new ServiceCentersInNearByCities(_objSCCache, NearByCitiesWidgetTopCount, _cityId, objVM.Make).GetData();

                MostPopularBikesWidget objPopularBikesWidget = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, false, true, PQSourceEnum.Mobile_ServiceCenter_Listing_CityPage, 0, _makeId);
                objPopularBikesWidget.TopCount = 9;
                objPopularBikesWidget.CityId = _cityId;
                objVM.PopularWidgetData = objPopularBikesWidget.GetData();

                objVM.UsedBikesByMakeList = BindUsedBikeByModel(objVM.City);

                objVM.DealersWidgetData = BindDealerWidget();

                objVM.BrandCityPopupWidget = new BrandCityPopupModel(EnumBikeType.ServiceCenter, (uint)_makeId, (uint)_cityId).GetData();

                BindPageMetas(objVM);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterCityPage.GetData()");
            }
            return objVM;
        }

        private void BindPageMetas(ServiceCenterCityPageVM objPageVM)
        {

            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.City != null && objPageVM.ServiceCentersListObject != null)
                {
                    objPageVM.PageMetaTags.Title = string.Format("{0} service centers in {1} | {0} bike servicing in {1} - BikeWale", objPageVM.Make.MakeName, objPageVM.City.CityName);
                    objPageVM.PageMetaTags.Keywords = string.Format("{0} servicing {1}, {0} service center in {1}, {0} Service centers, {0} service schedules, {0} bike repair, repairing, servicing", objPageVM.Make.MakeName, objPageVM.City.CityName);
                    objPageVM.PageMetaTags.Description = string.Format("There are {0} {1} service centers in {2}. Get in touch with your nearest {1} service center for service repairing, schedule details, pricing, pick and drop facility. Check the Service schedule for {1} bikes now.", objPageVM.ServiceCentersListObject.Count, objPageVM.Make.MakeName, objPageVM.City.CityName);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterIndiaPage.BindPageMetas()");
            }
        }

        private void ProcessQuery(string makeMaskingName, string cityMaskingName)
        {
            objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);
            _cityId = CitiMapping.GetCityId(cityMaskingName);

            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200 && _cityId > 0)
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

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterIndiaPage.BindUsedBikeByModel()");
            }
            return UsedBikeModel;
        }

        private DealersEntity BindDealerWidget()
        {
            DealersEntity objDealerList = null;
            try
            {
                objDealerList = _objDealerCache.GetDealerByMakeCity(_cityId, _makeId);

                objDealerList.Dealers = objDealerList.Dealers.Take((int)BikeShowroomWidgetTopCount);
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomDealerDetail.BindOtherDealerWidget()");
            }
            return objDealerList;
        }
    }
}